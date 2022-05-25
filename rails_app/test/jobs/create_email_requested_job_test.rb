require "test_helper"

class CreateEmailRequestedJobTest < ActiveJob::TestCase
  setup do
    @email = Email.new(address: 'test-from-job@example.com')
  end

  test "inserts email" do
    assert_difference("Email.count") do
      CreateEmailRequestedJob.perform_now(@email.to_json)
    end
  end
end
