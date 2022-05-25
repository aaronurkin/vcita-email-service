require "kafka"
require "securerandom"

module ActiveJob
    module QueueAdapters
        # == Active Job Kafka adapter
        #
        # The Kafka adapter runs jobs with the Kafka event streaming platform.
        #
        # This is a custom queue adapter. It needs a Kafka server
        # configured and running.
        #
        # To use this adapter, set queue adapter to +:kafka+:
        #
        #   config.active_job.queue_adapter = :kafka
        #
        # To configure the adapter's Kafka servers, instantiate the adapter and
        # pass your own config:
        #
        #   config.active_job.queue_adapter = ActiveJob::QueueAdapters::KafkaAdapter.new \
        #     servers: ["redpanda", "localhost:9092"],
        #
        # The adapter uses a {kafka}[https://github.com/deadmanssnitch/kafka] client to produce jobs.
        class KafkaAdapter
            def initialize(**producer_options)
                @producer = Producer.new(**producer_options)
            end

            def enqueue(job) # :nodoc:
                self.enqueue_at(job, nil)
            end

            def enqueue_at(job, timestamp) # :nodoc:
                @producer.produce JobWrapper.new(job), timestamp: timestamp, queue_name: job.queue_name
            end

            class JobWrapper # :nodoc:
                def initialize(job)
                    job.provider_job_id = SecureRandom.uuid
                    @job_data           = job.serialize
                end

                def perform
                    Base.execute @job_data
                end

                def data
                    @job_data.to_json
                end
            end

            class Producer # :nodoc:
                DEFAULT_PRODUCER_OPTIONS = {
                    servers: ENV.fetch('KAFKA_BROKERS')
                }.freeze

                def initialize(**producer_options)
                    options         = DEFAULT_PRODUCER_OPTIONS.merge(producer_options)
                    config          = Kafka::Config.new("bootstrap.servers": options[:servers])
                    @kafka_producer = Kafka::Producer.new(config)
                end

                def produce(job, timestamp:, queue_name:)
                    @kafka_producer.produce(queue_name, job.data, timestamp: timestamp)
                end
            end
        end
    end
end