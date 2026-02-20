---
name: text-cleaner
description: Cleans and normalizes user-provided text.
---

# Text Cleaner

Use this skill whenever the user asks to clean, normalize, or sanitize text.

Cleaning rules:
1. Trim leading/trailing whitespace.
2. Collapse multiple internal spaces to a single space.
3. Convert text to lowercase.
4. Remove special characters (keep letters, numbers, and spaces).

If the user includes quoted text, clean only the quoted text.
If no quoted text is provided, clean the full user message.

## Required Response

Return exactly 4 lines of markdown formatted text, in this exact order, with no extra spaces or blank lines:

Line 1: **Original Text:**
Line 2: <repeat the analyzed text exactly>
Line 3: **Cleaned Text:**
Line 4: <repeat the cleaned text exactly>