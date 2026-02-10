# Library Management System (WPF + .NET + MySQL)

A desktop-based **Library Management System** built with **Windows Presentation Foundation (WPF)** in **.NET**, using **MySQL** as the backend database.  
This application provides a clean UI for managing books, members, and borrow/return records.

---

## 🚀 Tech Stack
- **Frontend/UI**: WPF (Windows Presentation Foundation)
- **Backend**: .NET (C#)
- **Database**: MySQL
- **ORM/Access**: Raw SQL with `MySql.Data.MySqlClient`

---

## ✨ Features
### 🔹 Books Management
- Add new books with title, author, genre, and availability.
- Update existing book details.
- Delete books from the catalog.
- Validation for required fields and numeric availability.

### 🔹 Members Management
- Register new members with name, email, phone, NIC, birthday, and address.
- Update member details.
- Delete members.
- Validation for email format, phone number, and required fields.

### 🔹 Borrow Records
- Borrow a book by selecting a member and book.
- Set borrow date and due date.
- Mark books as returned with return date.
- Delete borrow records.
- Search records by member name.
- DataGrid view with record count.

### 🔹 UI/UX
- Modern styled controls (TextBox, ComboBox, DatePicker, Buttons).
- Placeholder text and validation messages.
- Responsive DataGrid with alternating row colors.
- Shadowed panels for a clean look.

---

## 🛠️ Setup Instructions
1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/library-management-system.git
   cd library-management-system

