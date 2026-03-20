/**
 * generate-lottery-numbers.js — Generates random lottery numbers for common lottery formats.
 *
 * Supports: Powerball, Mega Millions, and custom configurations.
 * Returns a structured object the AI uses to build the lottery response.
 */

/**
 * Picks `count` unique random integers in the range [min, max] (inclusive).
 * @param {number} count
 * @param {number} min
 * @param {number} max
 * @returns {number[]} sorted ascending
 */
function pickUnique(count, min, max) {
  const pool = [];
  for (let i = min; i <= max; i++) pool.push(i);
  for (let i = pool.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [pool[i], pool[j]] = [pool[j], pool[i]];
  }
  return pool.slice(0, count).sort((a, b) => a - b);
}

/**
 * Generates lottery numbers for the requested format.
 *
 * @param {"powerball"|"megamillions"|"custom"} type - Lottery type
 * @param {object} [customOptions] - Required when type is "custom"
 * @param {number} [customOptions.mainCount=6] - Number of main balls to pick
 * @param {number} [customOptions.mainMin=1] - Minimum value for main balls
 * @param {number} [customOptions.mainMax=49] - Maximum value for main balls
 * @param {number} [customOptions.bonusCount=0] - Number of bonus balls (0 = none)
 * @param {number} [customOptions.bonusMin=1] - Minimum value for bonus ball
 * @param {number} [customOptions.bonusMax=10] - Maximum value for bonus ball
 * @param {string} [customOptions.bonusLabel="Bonus"] - Label for the bonus ball
 * @returns {{ type: string, mainBalls: number[], bonusBall: number|null, bonusLabel: string|null }}
 */
function generateLotteryNumbers(type = 'powerball', customOptions = {}) {
  const formats = {
    powerball: {
      label: 'Powerball',
      mainCount: 5, mainMin: 1, mainMax: 69,
      bonusCount: 1, bonusMin: 1, bonusMax: 26, bonusLabel: 'Powerball'
    },
    megamillions: {
      label: 'Mega Millions',
      mainCount: 5, mainMin: 1, mainMax: 70,
      bonusCount: 1, bonusMin: 1, bonusMax: 25, bonusLabel: 'Mega Ball'
    },
    custom: {
      label: 'Custom',
      mainCount: customOptions.mainCount ?? 6,
      mainMin: customOptions.mainMin ?? 1,
      mainMax: customOptions.mainMax ?? 49,
      bonusCount: customOptions.bonusCount ?? 0,
      bonusMin: customOptions.bonusMin ?? 1,
      bonusMax: customOptions.bonusMax ?? 10,
      bonusLabel: customOptions.bonusLabel ?? 'Bonus'
    }
  };

  const format = formats[type] ?? formats.powerball;
  const mainBalls = pickUnique(format.mainCount, format.mainMin, format.mainMax);
  const bonusBall = format.bonusCount > 0
    ? pickUnique(1, format.bonusMin, format.bonusMax)[0]
    : null;

  return {
    type: format.label,
    mainBalls,
    bonusBall,
    bonusLabel: format.bonusCount > 0 ? format.bonusLabel : null
  };
}

module.exports = { generateLotteryNumbers };
