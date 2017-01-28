# Steam Reaction

Routinely query Steam's API for app updates; when an updated is detected associated web hooks are triggered.

### Supported Triggers
* SteamAppUpdate - Activates when a specified Steam App updates.
* TimeElapsed - Activates when a minimum specified amount of time elapses since the webhooks were last triggered.

### Supported Web Hooks
* Docker HUB - Trigger automated build on Docker HUB.
* SLACK - Post a message in SLACK.

### Working Concepts

Execute wehook(s) when triggers(s) are activated:

`[activated] Trigger(s) -=> [execute] Webhook(s)`

If no triggers activate no webhooks execute.

This process happens inside of a reactor which establishes many-to-many relationships between triggers and webhooks.

`reactor { [activated] Trigger(s) -=> [execute] Webhook(s) }`

Inside of a reactor any *single* activated trigger will result in the execution of *all* webhooks.

```
            Trigger 1    /==> [execute] Webhook 1
[activated] Trigger 2 ----==> [execute] Webhook 2
            Trigger n    \==> [execute] Webhook n
```


Reactors separate "business" concerns; use them to group related webhooks to all relevant triggers.

```
reactor 1 { SteamAppUpdate(app: X) ----------- Docker Hub build
                                          \--- SLACK channel message }

reactor 2 { SteamAppUpdate(app: Y) ------\/--- Docker Hub build
            TimeElapsed(time: X days) ---/\--- SLACK channel message }

reactor 3 { SteamAppUpdate(app: A) ----------- SLACK channel message
            SteamAppUpdate(app: B) ------/                           }
```

