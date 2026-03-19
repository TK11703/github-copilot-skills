/**
 * get-force-alignment.js — Sentiment to Force alignment mapper for the Yoda skill.
 *
 * Maps a detected sentiment value to a Star Wars Force alignment and
 * the corresponding narrative path label.
 */

const ALIGNMENTS = {
  positive: { alignment: 'Light', path: 'Path of the Jedi' },
  negative: { alignment: 'Dark',  path: 'Path of the Sith' },
  neutral:  { alignment: 'Grey',  path: 'Path not yet written' },
};

/**
 * Returns the Force alignment and path for a given sentiment.
 * @param {string} sentiment - 'positive' | 'negative' | 'neutral'
 * @returns {{ alignment: string, path: string }}
 */
function getForceAlignment(sentiment) {
  return ALIGNMENTS[sentiment.toLowerCase()] ?? ALIGNMENTS.neutral;
}

module.exports = { getForceAlignment };
