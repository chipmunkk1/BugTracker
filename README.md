# Full-Stack Bug Tracker 🐛

**[Live Demo on Azure] (Insert your link here)** | **[Video Walkthrough] (Insert your video link here)**

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
![Login and Signup](./image_598c34.png)

### 2. Isolated Workspaces
Once authenticated, users are prompted to join a workspace. Teams can create a brand new group (which generates a unique 9-digit access code) or join an existing project to collaborate on the same issue board.
![Join or Create Group](./image_598bdb.png)

### 3. Clean Reporting Dashboard
The main dashboard features a streamlined reporting form. Users can document exactly where the bug occurred (File Name), provide a detailed description, and assign a severity score (1-10).
![Report a Bug Dashboard](./image_59323c.png)

### 4. Dynamic Active Bug Board
Active bugs are fetched asynchronously via the .NET Core API. The UI dynamically color-codes issues based on their severity level (e.g., Level 10 Critical bugs appear in red, Medium severity in yellow). This allows developers to assess board priorities at a glance.
![Color-coded Active Bugs](./image_59325a.png)
*(Note: Earlier iterations of the UI focused on structural data mapping before the dynamic color-coding and modern navigation were introduced, as seen [here](./image_32b226.png)).*

### 5. Issue Resolution & History
When a bug is squashed, users can mark it as solved. This removes it from the active board and sends it to the "Solved Bugs Checklist," maintaining a clean workspace while preserving project history.
![Solved Bugs Checklist](./image_5931a3.png)

---

## ⚙️ Architecture & Backend Highlights
* **Micro-ORM Integration:** Utilized **Dapper** to execute raw, highly optimized SQL queries against the PostgreSQL database, ensuring fast read/write speeds and clean mapping to C# Data Transfer Objects (DTOs).
* **Asynchronous API:** Built a decoupled RESTful Web API using C# .NET Core to handle all database operations, allowing the frontend to update the UI asynchronously via the JavaScript Fetch API without page reloads.
* **Cloud Infrastructure:** The application logic is hosted on Azure Web Services, securely connected to a remote PostgreSQL database hosted on Aiven via encrypted connection strings.

## 🚀 Getting Started (Local Development)
To run this project locally:

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/YourUsername/BugTracker.git](https://github.com/YourUsername/BugTracker.git)
