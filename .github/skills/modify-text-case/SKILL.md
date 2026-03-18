---
name: modify-text-case
description: Converts selected or provided text to uppercase, lowercase, or PascalCase.
---

# Modify Text Case

Use this skill whenever the user asks to change text casing.

Primary invocation format:
- /case \<type\> \<text\>

Accepted \<type\> values (case-insensitive):
- upper
- uppercase
- lower
- lowercase
- pascal
- pascalcase

Also trigger on natural language phrases like (case-insensitive):
- uppercase
- lower case
- lowercase
- pascal case

Text to transform:
1. If text is selected, transform the selected text only.
2. Otherwise, if quoted text is provided, transform the quoted text only.
3. Otherwise, transform the full user message.

Case rules:
1. Uppercase: convert all letters to uppercase.
2. Lowercase: convert all letters to lowercase.
3. PascalCase: split words on whitespace, underscores, and hyphens; remove other separators; capitalize each word; concatenate without spaces.

If the requested case is ambiguous, ask one concise clarification question.
If the user uses /case but omits \<type\>, ask one concise clarification question for the type.

## Required Response

Return output in this exact structure:
Line 1: SKILL_USED:
Line 2+: <transformed text exactly, preserving line breaks>

Do not include labels, markdown formatting, quotes, code fences, or explanations.
