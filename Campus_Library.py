
import pyodbc
from datetime import date, timedelta
import random


def Connection_string():
    connection_string = (
        "DRIVER={ODBC Driver 17 for SQL Server};"
        "SERVER=ACU-HYD-LT-2032;"
        "DATABASE=LibraryDB;"
        "Trusted_Connection=yes;"
        "Encrypt=yes;"
        "TrustServerCertificate=yes;"
        "Pooling=no;"
        "MultipleActiveResultSets=no;"
    )

    conn = pyodbc.connect(connection_string)
    cursor = conn.cursor()

    cursor.execute("SELECT * FROM members")
    member_ids = [row[0] for row in cursor.fetchall()]

    cursor.execute("SELECT * FROM books")
    book_ids = [row[0] for row in cursor.fetchall()]

    if not member_ids or not book_ids:
        print("No members or books found.")
        cursor.close()
        conn.close()
        return

    cursor.execute("""
        IF OBJECT_ID('issued_books', 'U') IS NULL
        BEGIN
            CREATE TABLE issued_books (
                issue_id INT IDENTITY(1,1) PRIMARY KEY,
                member_id INT NOT NULL,
                book_id INT NOT NULL,
                issue_date DATE NOT NULL,
                due_date DATE NOT NULL,
                return_date DATE NULL,
                fine INT NOT NULL DEFAULT 0,
                FOREIGN KEY (member_id) REFERENCES members(member_id),
                FOREIGN KEY (book_id) REFERENCES books(book_id)
            )
        END
    """)

    today = date.today()

    for i in range(60):
        member_id = random.choice(member_ids)
        book_id = random.choice(book_ids)

        issue_date = today - timedelta(days=14)
        due_date = issue_date + timedelta(days=14)

        if i % 3 == 0:
            return_date = None
            fine = 0

        elif i % 3 == 1:
            return_date = issue_date + timedelta(
                days=random.randint(0, 5)
            )
            fine = 0

        else:
            return_date = due_date + timedelta(
                days=random.randint(1, 5)
            )
            overdue_days = (return_date - due_date).days
            fine = overdue_days * 2

        cursor.execute("""
            INSERT INTO issued_books
                (member_id, book_id, issue_date, due_date,
                 return_date, fine)
            VALUES (?, ?, ?, ?, ?, ?)
        """, member_id, book_id, issue_date, due_date,
             return_date, fine)

    conn.commit()
    print("60 records inserted successfully.")

    cursor.close()
    conn.close()


Connection_string()
