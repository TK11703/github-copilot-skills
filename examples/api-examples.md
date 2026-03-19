# API Ideas for GitHub Copilot Skills Demo

## 🌱 1. Micro-Utility APIs

> Fast to build, great for showing Copilot's codegen

These are tiny, self-contained endpoints that demonstrate:

- Model binding
- Validation
- Minimal API patterns
- OpenAPI annotations

### Ideas

| API | Description | Why it demos well |
|-----|-------------|-------------------|
| `GET /uuid/new` | Returns a new GUID + timestamp | Shows simple utility endpoints |
| `GET /math/prime/{n}` | Returns whether n is prime | Great for showing Copilot generating logic |
| `POST /text/slugify` | POST text → returns URL-safe slug | Copilot can write the transformation |
| `GET /convert/fahrenheit-to-celsius` | Simple numeric conversion | Good for showing parameter binding |

---

## 📚 2. Knowledge / Content APIs

> Let Copilot generate domain logic

These are fun because Copilot can generate the "knowledge" behind them.

### Ideas

| API | Description | Why it demos well |
|-----|-------------|-------------------|
| `GET /recipes/suggest?ingredients=…` | Returns a simple recipe idea | Copilot can invent sample data |
| `GET /books/recommendations?genre=…` | Returns 3–5 book suggestions | Great for showing enum binding |
| `GET /quotes/random` | Returns a random inspirational quote | Copilot can generate the quote list |

---

## 🧩 3. Stateful Mini-Systems

> Slightly more complex, great for CRUD demos

These show:

- EF Core or in-memory stores
- CRUD patterns
- DTOs and validation

### Ideas

| API | Description | Why it demos well |
|-----|-------------|-------------------|
| `CRUD /notes` | Simple note-taking CRUD | Very approachable |
| `CRUD /habits` | Track daily habits | Shows date handling |
| `CRUD /expenses` | Track small expenses | Good for decimal + validation |

---

## 🎮 4. Fun / Interactive APIs

> Great for live demos

These are playful and memorable.

### Ideas

| API | Description | Why it demos well |
|-----|-------------|-------------------|
| `GET /games/roll-dice` | Returns a random dice roll | Shows randomness + simple models |
| `GET /games/rock-paper-scissors` | User chooses; API returns result | Copilot can generate the logic |
| `GET /games/word-scramble` | Returns a scrambled version of a word | Fun transformation logic |

---

## 🛠️ 5. Developer-Tooling APIs

> Perfect for a GitHub Copilot Skills demo

These feel "meta" and show off automation.

### Ideas

| API | Description | Why it demos well |
|-----|-------------|-------------------|
| `POST /dev/regex-test` | `{ pattern, input }` → returns matches | Copilot can generate regex helpers |
| `POST /dev/json/pretty` | Pretty-prints JSON | Great for showing model binding |
| `GET /dev/headers` | Returns request headers | Shows request context access |

---

## ⭐ Top 3 Picks for Your Demo

If you want APIs that really show off Copilot Skills in VS Code:

1. **Rock-Paper-Scissors API** — Copilot can generate the entire game logic + OpenAPI docs.
2. **Slugify Text API** — Great for showing transformations and unit tests.
3. **Recipe Suggestion API** — Copilot can invent sample data, models, and filtering logic.