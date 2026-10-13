## Commit Message Rules

- Always write commit messages in English.
- Generate the entire commit message (subject, body, and footer) in English, regardless of the language used in the chat or the code comments.
- Follow the Conventional Commits specification: `<type>(<scope>): <description>`
- Allowed types: feat, fix, docs, style, refactor, perf, test, build, ci, chore, revert
- Scope is optional; use a short noun describing the affected area (e.g., `io`, `ui`, `unpacker`).
- Subject line:
  - Use the imperative mood (e.g., "add", "fix", not "added", "fixes").
  - Keep it within 50 characters where possible (72 max).
  - Start with a lowercase letter and do not end with a period.
- Add a body when needed, separated by a blank line, explaining what and why (not how). Wrap at 72 characters.
- For breaking changes, add `!` after the type/scope (e.g., `feat(io)!: ...`) and/or a `BREAKING CHANGE:` footer.
- Reference issues in the footer when applicable (e.g., `Closes #123`).
