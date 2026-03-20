---
name: lottery-numbers
description: Generates random lottery numbers for Powerball, Mega Millions, or a custom lottery format. Use when user asks to pick lottery numbers, generate lotto numbers, or requests a lottery pick. Trigger phrases include "lottery numbers", "lotto numbers", "pick my numbers", "powerball numbers", "mega millions numbers", "/lottery".
---

# Lottery Numbers

Use this skill **whenever** the user message matches any of these (case-insensitive):
- lottery numbers
- lotto numbers
- pick my numbers
- pick lottery numbers
- generate lottery numbers
- powerball numbers
- mega millions numbers
- /lottery

## How It Works

1. Detect the lottery type from the user's message:
   - **"powerball"** → use `type = "powerball"`
   - **"mega millions"** → use `type = "megamillions"`
   - **custom** (user specifies count/range) → use `type = "custom"` with appropriate `customOptions`
   - **no type specified** → default to `type = "powerball"`

2. Execute the `generateLotteryNumbers` function exported from [script](./scripts/generate-lottery-numbers.js):
   ```
   generateLotteryNumbers(type, customOptions?)
   ```

3. The script returns this JSON shape:
   ```json
   {
     "type": "Powerball",
     "mainBalls": [4, 17, 29, 41, 55],
     "bonusBall": 12,
     "bonusLabel": "Powerball"
   }
   ```
   When there is no bonus ball, `bonusBall` and `bonusLabel` will be `null`.

## Custom Options

If the user specifies a custom format, pass a `customOptions` object:

| Option | Default | Description |
|--------|---------|-------------|
| `mainCount` | `6` | Number of main balls to draw |
| `mainMin` | `1` | Minimum main ball value |
| `mainMax` | `49` | Maximum main ball value |
| `bonusCount` | `0` | Number of bonus balls (0 = none) |
| `bonusMin` | `1` | Minimum bonus ball value |
| `bonusMax` | `10` | Maximum bonus ball value |
| `bonusLabel` | `"Bonus"` | Label for the bonus ball |

## Required Response

Return output in this exact structure:

Line 1: **🎰 <`result.type`> Numbers**
Line 2: **Your numbers:** <`result.mainBalls` joined by spaces, each in bold>
Line 3 (only if `result.bonusBall` is not null): **<`result.bonusLabel`>:** <`result.bonusBall` in bold>
Line 4: *(blank line)*
Line 5: *Good luck! 🍀*

### Example Output (Powerball)

**🎰 Powerball Numbers**
**Your numbers:** **4** **17** **29** **41** **55**
**Powerball:** **12**

*Good luck! 🍀*

### Example Output (no bonus ball)

**🎰 Custom Numbers**
**Your numbers:** **3** **11** **22** **34** **40** **47**

*Good luck! 🍀*
