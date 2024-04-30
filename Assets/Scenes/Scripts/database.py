import sqlite3
import os
from typing import Tuple
import re

def initialize_database(db_name='university_schedule.db'):
    conn = sqlite3.connect(db_name)
    c = conn.cursor()
    
    # Create the Department table
    c.execute('''CREATE TABLE IF NOT EXISTS Department (
                dept_id TEXT PRIMARY KEY)''')

    # Create the Classroom table with room_number and building as a composite primary key
    c.execute('''CREATE TABLE IF NOT EXISTS Classroom (
                room_number TEXT,
                building TEXT NOT NULL,
                PRIMARY KEY (building, room_number))''')

    # Create the Course table
    c.execute('''CREATE TABLE IF NOT EXISTS Course ( 
                course_id INTEGER,
                dept_id TEXT NOT NULL,
                PRIMARY KEY (course_id, dept_id),
                FOREIGN KEY (dept_id) REFERENCES Department(dept_id))''')

    # Create the CourseTimeSlot table with room_number and building added
    c.execute('''CREATE TABLE IF NOT EXISTS CourseTimeSlot (
                timeslot_id INTEGER PRIMARY KEY AUTOINCREMENT,
                course_id INTEGER NOT NULL,
                dept_id TEXT NOT NULL,
                room_number TEXT NOT NULL, 
                building TEXT NOT NULL,
                start_time TEXT NOT NULL,
                end_time TEXT NOT NULL,
                day_of_week TEXT NOT NULL,
                FOREIGN KEY (course_id, dept_id) REFERENCES Course(course_id, dept_id),
                FOREIGN KEY (building, room_number) REFERENCES Classroom(building, room_number))''')

    conn.commit()
    conn.close()

initialize_database()

def contains_whitespace(s):
    return ' ' in s or '\t' in s or '\n' in s

# Function to split class into dept_id and course_id
def split_class(class_name: str) -> Tuple[str, str]:
    # Use regex or any other suitable method to split the class into dept and course
    parts = class_name.split()
    if parts[0] == "Exam":
        return "Exam", "1"
    if contains_whitespace(class_name):
        return parts[0], parts[1]

    return split_class_no_space(class_name)

def split_class_no_space(class_name: str) -> Tuple[str, str]:
    # Use regex to find the first occurrence of numbers and split the department and course ID
    match = re.match(r"([a-zA-Z]+)(\d+)", class_name)
    # Check if the pattern was matched
    if match:
        items = match.groups()
        return items[0], items[1]  # dept_id, course_id
    else:
        raise ValueError(f"Invalid class format: {class_name}")

def parse_classroom_name(classroom_name):
    match = re.match(r"([a-zA-Z]+)(\d+[a-zA-Z]*)", classroom_name)
    if match:
        building, room_number = match.groups()
        return building, room_number
    else:
        raise ValueError(f"Unable to parse classroom name: {classroom_name}")


new_db_name = 'university_schedule.db'
db_directory = '/Users/wilbercortez/Documents/Uni-Vision/Assets/StreamingAssets/Resources/db'
weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday']

main_conn = sqlite3.connect(new_db_name)
main_cursor = main_conn.cursor()

for file in os.listdir(db_directory):
    if file.endswith('.db'):
        old_db_path = os.path.join(db_directory, file)
        classroom_name = file.replace('.db', '')
        building, room_number = parse_classroom_name(classroom_name)
        old_conn = sqlite3.connect(old_db_path)
        old_cursor = old_conn.cursor()

        for day in weekdays:
            old_cursor.execute(f"SELECT name FROM sqlite_master WHERE type='table' AND name='{day}'")
            if old_cursor.fetchone():
                old_cursor.execute(f'SELECT start_date, end_date, class FROM {day}')
                rows = old_cursor.fetchall()

                for start_date, end_date, class_name in rows:
                    dept_id, course_id = split_class(class_name)
                    main_cursor.execute('INSERT OR IGNORE INTO Department (dept_id) VALUES (?)', (dept_id,))
                    main_cursor.execute('INSERT OR IGNORE INTO Classroom (room_number, building) VALUES (?, ?)', (room_number, building))
                    main_cursor.execute('INSERT OR IGNORE INTO Course (course_id, dept_id) VALUES (?, ?)', (course_id, dept_id))
                    main_cursor.execute('INSERT INTO CourseTimeSlot (course_id, dept_id, room_number, building, start_time, end_time, day_of_week) VALUES (?, ?, ?, ?, ?, ?, ?)', (course_id, dept_id, room_number, building, start_date, end_date, day))

        old_conn.commit()
        old_conn.close()

main_conn.commit()
main_conn.close()
print('Database transfer is complete.')
