---
name: sentiment-detector
description: Performs simple sentiment classification on user-provided text.
---

# Sentiment Detector

Use this skill **whenever** the user asks for sentiment analysis/classification, including phrases like (case-insensitive):
- what is this text's sentiment
- what's the sentiment
- sentiment of this text
- classify sentiment
- analyze sentiment

If the user includes quoted text, analyze the quoted text.
If no quoted text is present, analyze the full user message.

## Required Response

Return exactly 5 lines of markdown formatted text, in this exact order, with no extra spaces or blank lines:

Line 1: **Original Text:**
Line 2: <repeat the analyzed text exactly>
Line 3: **Sentiment:** <Positive | Negative | Neutral>
Line 4: **Why:**
Line 5: <brief explanation>