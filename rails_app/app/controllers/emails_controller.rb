class EmailsController < ApplicationController
  before_action :set_email, only: %i[ show update destroy ]

  # GET /emails
  def index
    @emails = Email.all

    render json: @emails
  end

  # GET /emails/1
  def show
    render json: @email
  end

  # POST /emails
  def create
    CreateEmailRequestedJob.perform_later(email_params.to_json)
    render status: :ok
  end

  # PATCH/PUT /emails/1
  def update
    if @email.update(email_params)
      render json: @email
    else
      render json: @email.errors, status: :unprocessable_entity
    end
  end

  # DELETE /emails/1
  def destroy
    @email.destroy
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_email
      @email = Email.find(params[:id])
    end

    # Only allow a list of trusted parameters through.
    def email_params
      params.require(:email).permit(:address)
    end
end
