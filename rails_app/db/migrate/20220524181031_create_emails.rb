class CreateEmails < ActiveRecord::Migration[7.0]
  def change
    create_table :emails do |t|
      t.string :address, null: false, index: { name: 'emails_unique_address', unique: true }

      t.timestamps
    end
  end
end
