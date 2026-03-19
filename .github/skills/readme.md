
# GitHub Copilot Skills

This folder contains the GitHub Copilot skills for this repository. Each skill is a self-contained directory with a `SKILL.md` file that teaches Copilot specialized behavior — from text transformations and sentiment analysis to domain-specific coding guidance. Skills can optionally include external scripts to handle logic that is better expressed in code.

## Table of Contents

- [What are GitHub Copilot Skills?](#what-are-github-copilot-skills)
- [Requirements](#requirements)
- [Benefits of Skills?](#benefits-of-skills)
- [Skills vs Instructions vs Prompts](#️-skills-vs-instructions-vs-prompts)
- [Folder Structure](#folder-structure)
- [Creating a Skill](#creating-a-skill)
- [Using External Scripts](#using-external-scripts)
- [Skills in This Repo](#skills-in-this-repo)
- [More Examples](#more-examples)

## What are GitHub Copilot Skills?

**GitHub Copilot Skills** are reusable, domain-specific instruction files that extend Copilot's behavior in VS Code. A skill is a `SKILL.md` file placed in a known location that Copilot reads before responding to a matched request. Skills allow you to:

- Define **trigger phrases** or **slash commands** (e.g. `/case`, `/yoda`) that activate specialized behavior.
- Encode **domain knowledge**, **response formats**, and **decision logic** directly into Copilot's context.
- Call **supporting scripts** (JavaScript, PowerShell, etc.) to perform computation that Copilot then incorporates into its response. Execution is sandboxed and only allowed after the user explicitly approves running local code.
- Establish **consistent output structures** so Copilot always returns responses in a predictable format.

## Requirements

| Requirement | Details |
|---|---|
| **VS Code** | Latest stable release |
| **GitHub Copilot extension** | Requires an active GitHub Copilot subscription |
| **GitHub Copilot Chat extension** | Required for agent/skill invocation |
| **Script runtime** | Only needed if a skill calls external scripts (e.g. Node.js for `.js`, Python for `.py`, PowerShell for `.ps1`) |

No additional configuration is required. Copilot automatically discovers skill files under `.github/skills/` when they are present in the open workspace.

## Benefits of Skills

GitHub Copilot Skills offer several advantages over ad-hoc prompting or general Copilot usage:

- **Consistency** — Skills enforce a fixed output format, so every invocation returns a predictable, structured response rather than a freeform answer.
- **Low Impact** - Skills are indexed as tools. Copilot loads only their metadata, so the runtime code is not injected into the context window. The skill is invoked only when needed. The skill’s description is small and lightweight.
- **Reusability** — Once authored, a skill can be reused across sessions and shared with teammates by committing it to the repository.
- **Domain encapsulation** — Complex domain knowledge, business rules, or coding standards are encoded once in `SKILL.md` rather than repeated in every prompt.
- **Script integration** — Skills can delegate computation to external scripts (JavaScript, PowerShell, etc.), allowing logic that is difficult to express in natural language to be handled in code.
- **Discoverability** — Skills are automatically discovered by Copilot when present in `.github/skills/`, with no additional configuration required. The skill’s description influences when Copilot thinks it’s appropriate to use it.
- **Reduced prompt engineering** — Trigger phrases and slash commands let users activate specialized behavior without writing long, precise prompts each time.

This means that skills:

- Don't consume context window space
- Don't slow down reasoning
- Don't conflict with each other
- Don't affect tone or behavior
- Scale infinitely better than instructions

Skills expand **capabilities**, not **cognitive load**.

## Skills vs. Instructions vs. Prompts

| Feature | Skills | Instructions | Prompts |
|---|---|---|---|
| **Purpose** | Add new capabilities via code | Shape Copilot's personality & behavior | Ask Copilot to do something once |
| **Lives in** | `.github/skills/<name>/SKILL.md` | GitHub/VS Code settings | Chat window |
| **Persistence** | Permanent, reusable | Persistent | Temporary |
| **Executes code?** | Yes (local runtime) | No | No |
| **Best for** | Automation, APIs, workflows, custom logic | Tone, preferences, rules | One-off tasks |
| **Affects chat context?** | Yes — by adding new callable actions Copilot can choose from | Yes — modifies Copilot’s baseline behavior across all chats | Yes — but only for the current turn or thread |
| **Example** | "Call my JS script to format JSON" | "Use concise explanations" | "Summarize this file" |

 - Instructions define how Copilot behaves
 - Skills define what Copilot can do
 - Prompts define what Copilot should do right now

## Folder Structure

The location where skills are discovered depends on the AI tool being used. GitHub Copilot looks for skills under `.github/skills/` by default — this path can be configured via the `chat.skills.enabled` setting in VS Code. Other tools such as Claude may use a different location (e.g. a root-level `skills/` folder or a tool-specific config directory). Check your tool's documentation for the exact path it scans.

Each skill lives in its own folder and contains at minimum a `SKILL.md` file, and optionally a `scripts/` subfolder for any supporting code. Folder names should be lowercase, with multiple words separated by dashes (e.g. `my-skill`, `get-weather`).

```
.github/
└── skills/
    ├── <skill-name>/
    │   ├── SKILL.md          ← Required: skill definition and instructions
    │   └── scripts/          ← Optional: external scripts called by the skill
    │       └── script.js
    └── ...
```

Full repository layout:

```
.github/
├── instructions/
│   ├── aspnet-rest-apis.instructions.md
│   └── csharp.instructions.md
└── skills/
    ├── aspnet-minimal-api-openapi/
    │   └── SKILL.md
    ├── dotnet-best-practices/
    │   └── SKILL.md
    ├── force-response/
    │   └── SKILL.md
    ├── hello-world/
    │   └── SKILL.md
    ├── modify-text-case/
    │   └── SKILL.md
    ├── sentiment-detector/
    │   └── SKILL.md
    ├── text-cleaner/
    │   └── SKILL.md
    └── yoda/
        ├── SKILL.md
        └── scripts/
            ├── handler.js
            └── script.js
src/
└── VerticalSlicesApi/          ← Sample .NET 9 Minimal API project
```

## Creating a Skill

1. Create a new folder under `.github/skills/` named after your skill (kebab-case recommended):
   ```
   .github/skills/my-skill/
   ```

2. Add a `SKILL.md` file (filename must be uppercase `SKILL` with a lowercase `.md` extension) with a YAML front matter header and your instructions. The `name` field in the front matter must match the skill's folder name exactly:
   ```markdown
   ---
   name: my-skill
   description: A short description of what this skill does.
   ---

   # My Skill

   Use this skill when the user asks about X.

   ## Trigger Phrases
   - /my-skill
   - do the thing

   ## Required Response

   Return output in this exact structure:
   Line 1: SKILL_USED:
   Line 2+: <your transformed output>
   ```

3. The `description` field in the front matter controls when Copilot considers the skill relevant. Make it specific and action-oriented.

4. Define clear **trigger phrases** or **slash commands** so Copilot knows exactly when to activate the skill.

5. Specify a **Required Response** section to lock in the output format.

## Using External Scripts

Some skills need to perform logic that is better expressed in code than in natural language — for example, sentiment analysis, text transformation, or data lookups. You can place supporting scripts in a `scripts/` subfolder next to `SKILL.md`.

**Example — the `yoda` skill:**

```
.github/skills/yoda/
├── SKILL.md
└── scripts/
    ├── handler.js    ← orchestrates trigger detection and calls script.js
    └── script.js     ← pure logic: sentiment scoring, force-alignment mapping
```

In `SKILL.md`, instruct Copilot to call the exported function and pass the user's message as an argument:

```markdown
## How It Works

1. Execute the `handle` function exported from [script](./scripts/handler.js).
2. Call: handle("<full user request including the trigger phrase>")
3. The script returns a JSON object — use its fields to build your response.
```

The script should export a single `handle(message)` function and return a plain JSON-serializable object. Copilot will interpret the returned value and weave it into the response according to your `SKILL.md` instructions.

**Tips for external scripts:**
- Keep scripts **pure and side-effect free** — they should accept input and return output only.
- Return a **structured JSON object** so Copilot can reliably access individual fields.
- Document the return shape in `SKILL.md` so Copilot knows exactly what properties to reference.
- Use `require()` / CommonJS modules for Node.js scripts to keep dependencies self-contained.

## Skills in This Repo

| Skill | Trigger | Description |
|---|---|---|
| `hello-world` | `hello world`, `hi there`, `hi world` | Returns a friendly greeting |
| `force-response` | `may the force be with you`, `mtfbwy` | Returns a Star Wars blessing |
| `get-weather` | `What's the weather in <city>?`, `Weather in <city>`, `/weather <city>` | Retrieves real-time weather information for a specified city |
| `modify-text-case` | `/case <type> <text>` | Converts text to `upper`, `lower`, or `pascal` case |
| `sentiment-detector` | `analyze sentiment`, `classify sentiment`, etc. | Classifies text as Positive, Negative, or Neutral |
| `text-cleaner` | `clean this text`, `normalize text`, etc. | Trims, collapses whitespace, lowercases, removes special characters |
| `yoda` | `Hey Yoda`, `Master Yoda`, `/yoda` | Responds in Yoda-style wisdom using an external JS script for sentiment and force-alignment |
| `dotnet-best-practices` | Applied to `**/*.cs` files | Reviews and enforces .NET/C# project conventions |
| `aspnet-minimal-api-openapi` | Applied to ASP.NET endpoint work | Guides creation of Minimal API endpoints with OpenAPI documentation |
| `microsoft-skill-creator` | "create a skill for...", "build a skill for..." | Creates hybrid skills for Microsoft technologies using Learn MCP tools |

## More Examples

Looking for more skill ideas and community examples? Check out these resources:

- **Awesome Copilot Skills** — A curated list of community-contributed GitHub Copilot skills:
  [github.com/github/awesome-copilot/tree/main/skills](https://github.com/github/awesome-copilot/tree/main/skills)

- **AwesomeSkill.ai** — A searchable gallery of Copilot skills across categories:
  [awesomeskill.ai](https://awesomeskill.ai/)

- **Anthropic Skills** — Example skills from Anthropic:
  [github.com/anthropics/skills/tree/main/skills](https://github.com/anthropics/skills/tree/main/skills)