import csv
import os
from github import Github

# Authenticate using the GitHub token
GITHUB_TOKEN = os.getenv("GH_PAT")
REPO_NAME = "workanvesh123/tmd_visualizer_webapi_refactor"  # Replace with your repository name
github_client = Github(GITHUB_TOKEN)
repo = github_client.get_repo(REPO_NAME)

if not GITHUB_TOKEN:
    raise ValueError("Missing GH_PAT! Did you set it as a GitHub secret?")

# Path to the CSV file
CSV_FILE_PATH = ".github/issues.csv"

# Function to create issues from the CSV
def create_issues_from_csv():
    with open(CSV_FILE_PATH, mode="r") as file:
        reader = csv.DictReader(file)
        for row in reader:
            title = row["Title"]
            body = row.get("Body", "")
            labels = [label.strip() for label in row.get("Labels", "").split(",") if label.strip()]
            assignees = [assignee.strip() for assignee in row.get("Assignees", "").split(",") if assignee.strip()]
            
            # Create the issue
            repo.create_issue(
                title=title,
                body=body,
                labels=labels,
                assignees=assignees
            )
            print(f"Issue '{title}' created successfully!")

if __name__ == "__main__":
    create_issues_from_csv()
