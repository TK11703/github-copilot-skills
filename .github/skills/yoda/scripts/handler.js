/**
 * handler.js — Trigger detection and response orchestration for the Yoda skill.
 *
 * Detects Yoda skill trigger phrases in user messages, infers sentiment,
 * and builds a structured context object for the AI to generate a Yoda response.
 */

const { getForceAlignment } = require('./get-force-alignment.js');

const TRIGGER_PHRASES = ['hey yoda', 'master yoda', 'master', '/yoda'];

/**
 * Extracts the user's actual question by stripping the trigger phrase.
 * @param {string} message
 * @returns {string}
 */
function extractQuestion(message) {
  let text = message;
  for (const phrase of TRIGGER_PHRASES) {
    const pattern = new RegExp(phrase.replace('/', '\\/'), 'i');
    text = text.replace(pattern, '').trim();
  }
  return text || message;
}

/**
 * Simple keyword-based sentiment inference.
 * Returns 'positive', 'negative', or 'neutral'.
 * @param {string} text
 * @returns {string}
 */
function inferSentiment(text) {
  const lower = text.toLowerCase();

  const positiveWords = ['help', 'kind', 'hope', 'love', 'trust', 'peace', 'learn', 'grow', 'save', 'protect'];
  const negativeWords = ['revenge', 'anger', 'hate', 'destroy', 'fear', 'betray', 'kill', 'attack', 'deceive', 'hurt'];

  const positiveScore = positiveWords.filter((w) => lower.includes(w)).length;
  const negativeScore = negativeWords.filter((w) => lower.includes(w)).length;

  if (positiveScore > negativeScore) return 'positive';
  if (negativeScore > positiveScore) return 'negative';
  return 'neutral';
}

/**
 * Orchestrates the full Yoda skill flow for a given user message.
 * Returns a context object the AI uses to build the final Yoda response.
 *
 * @param {string} message - The raw user message
 * @returns {{ question: string, sentiment: string, alignment: string, path: string }}
 */
function handle(message) {
  
  const question = extractQuestion(message);
  const sentiment = inferSentiment(question);
  const { alignment, path } = getForceAlignment(sentiment);

  return { question, sentiment, alignment, path };
}

module.exports = { extractQuestion, inferSentiment, handle };
