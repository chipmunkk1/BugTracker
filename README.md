# Full-Stack Bug Tracker 

**[Live Demo on Azure] (https://bug-tracker-front-end-chi.vercel.app/index.html))

A full-stack bug tracking application designed to help development teams log, manage, and resolve project issues collaboratively. This platform provides isolated workspaces, real-time issue tracking, and dynamic severity color-coding to help teams prioritize critical bugs.

## 💻 Tech Stack
* **Frontend:** Vanilla JavaScript, HTML5, CSS3
* **Backend:** C# .NET Core Web API
* **Database:** PostgreSQL (Hosted on Aiven)
* **ORM:** Dapper (Micro-ORM for optimized raw SQL execution)
* **Hosting/Deployment:** Azure Web Services

---

## 📸 Application Walkthrough

### 1. Secure Authentication
Users must create an account and log in to access team workspaces. The app uses secure state management to maintain the user's session.
![Login and Signup] <img width="1899" height="944" alt="image" src="https://github.com/user-attachments/assets/a131ade1-40bd-4613-b3e4-2e3072cc2bab" />



### 2. Isolated Workspaces
Once authenticated, users are prompted to join a workspace. Teams can create a brand new group (which generates a unique 9-digit access code) or join an existing project to collaborate on the same issue board.
![Join or Create Group]<img width="1908" height="950" alt="image" src="https://github.com/user-attachments/assets/10f2bc72-4873-43db-bb07-aad9256aac5a" />


### 3. Clean Reporting Dashboard
The main dashboard features a streamlined reporting form. Users can document exactly where the bug occurred (File Name), provide a detailed description, and assign a severity score (1-10).
![Report a Bug Dashboard] <img width="1885" height="746" alt="image" src="https://github.com/user-attachments/assets/362fde86-3eb1-455a-873c-603bfbf731b0" />


### 4. Dynamic Active Bug Board
Active bugs are fetched asynchronously via the .NET Core API. The UI dynamically color-codes issues based on their severity level (e.g., Level 10 Critical bugs appear in red, Medium severity in yellow). This allows developers to assess board priorities at a glance.
![Color-coded Active Bugs](./image_59325a.png)
*(Note: Earlier iterations of the UI focused on structural data mapping before the dynamic color-coding and modern navigation were introduced, as seen [here]<img width="1865" height="624" alt="image" src="https://github.com/user-attachments/assets/ff2e4686-9ccc-4062-b5fb-da08c5344b44" />


### 5. Issue Resolution & History
When a bug is squashed, users can mark it as solved. This removes it from the active board and sends it to the "Solved Bugs Checklist," maintaining a clean workspace while preserving project history.
![Solved Bugs Checklist]<img width="1908" height="918" alt="image" src="https://github.com/user-attachments/assets/2d4fbb6b-9479-431b-bce9-4d7df284a9b2" />


---

## ⚙️ Architecture & Backend Highlights
* **Micro-ORM Integration:** Utilized **Dapper** to execute raw, highly optimized SQL queries against the PostgreSQL database, ensuring fast read/write speeds and clean mapping to C# Data Transfer Objects (DTOs).
* **Asynchronous API:** Built a decoupled RESTful Web API using C# .NET Core to handle all database operations, allowing the frontend to update the UI asynchronously via the JavaScript Fetch API without page reloads.
* **Cloud Infrastructure:** The application logic is hosted on Azure Web Services, securely connected to a remote PostgreSQL database hosted on Aiven via encrypted connection strings.
