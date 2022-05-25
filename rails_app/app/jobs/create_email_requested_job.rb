class CreateEmailRequestedJob < ApplicationJob
  queue_as :create_email_requested

  def perform(request_json)
    request = JSON.parse(request_json, symbolize_names: true)
    created = Email.new(request)

    unless created.save
      puts "FAILED inserting email: #{request}"
      return
    end

    puts "created: #{created}"
  end
end
