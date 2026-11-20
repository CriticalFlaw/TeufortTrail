# Patch Notes: Game Balance Overhaul

The trail should now be hard but fair. Deaths come from wearing the team down over time, not from one unlucky dice roll. All tuning values now live in one new file, `Entities/GameBalance.cs`, so the game is easy to rebalance later.

## Food and rations
- **Fixed:** the party ate far too much food. Each member ate the ration amount times the whole party size, so a team of four ate four times what it should have. Each member now eats only their own share:
  - Filling: 5 lb per person per day
  - Meager: 3 lb
  - Bare: 2 lb
- The party now eats on days spent away from the trail: hunting, trading, waiting for the ferry, crossing rivers and time-skip events.
- Small rations now cost health. On Meager there's an 8% chance per day of losing 10–30 health. On Bare it's 30%.
- Eating restores health, but only up to Good, and only for members who aren't sick or injured. Filling heals 1–6 per day, Meager heals 0–3 and Bare heals nothing.
- Starving now costs 20–45 health per day (was 10–50).

## Illness and injury
- Healthy members now have a small daily chance of falling ill: 0.5% to start, +3% without enough hats, +0.6% on Meager and +2% on Bare.
- **Hats matter:** each living member needs 2 hats to stay warm.
- While travelling, a sick or injured member has a 6% daily chance to recover and a 5% chance to take a turn for the worse. Otherwise, they have a 40% chance to lose 10–35 health.
- "Well again" now cures the illness but no longer restores full health. Lost health has to come back through eating and resting.
- "Turn for the worse" now deals 130 damage. Weak members can die from it, and the message says so when they do.
- **Fixed:** taking damage could trigger a new illness or injury on someone who already had one. That caused chains of events. Damage from an existing illness no longer causes new ones.

## Resting
- Each day of rest, a member eats, recovers 15–35 health and has a 30% chance to shake off an illness or injury.
- Resting without food now costs 15–40 health per day.
- The rest summary now shows the party's status.

## Random events
- There's now at most one random event per travel day. Each day rolls a 7% chance for a wild event, 3% for a vehicle event and 3% for an animal event. Animal events hit a random living member.
- **Fixed:** every event was registered under every category. For example, a yeti attack could happen as a "vehicle" event. Events now only happen in their own category.
- Disasters now take at most 30% of any one supply, instead of up to all of it.
- Money is only lost to thieves and river floods.
- Found supplies (abandoned trucks) never include money, and the amounts are smaller.
- **Events hurt instead of instantly killing.** Each member rolls separately to be injured, and only dies if the damage brings their health down to zero:

  | Event | Chance to be hurt | Damage |
  |---|---|---|
  | Yeti attack | 25% | 60–160 |
  | Van floods | 30% | 60–180 |
  | Van fire | 25% | 40–120 |
  | Crash | 20% | 20–80 |
  | Robot ambush (10+ ammo) | 15% | 30–90 |
  | Robot ambush (under 10 ammo) | 40% | 60–160 |
  | Spy theft | 10% | 20–60 |

- **Robot ambush:** with at least 10 ammo, the party fights the robots off. Without it, the party barely fights back. Each ambush uses 5–15 ammo.
- Event messages are rewritten. They now list lost items with readable names ("3 hats", "$40"), say "Luckily, no supplies were lost" when nothing was taken, and say who was injured and who died.

## Rivers
- **Fixed:** crossings could charge the wrong payment. The ferry now always costs money and help from the locals always costs hats, depending on what you chose.
- Fording now rolls for a flood once, halfway across. The chance grows with the river's width, from 10% on the narrowest rivers to 55% on the widest.
- The river screen now hints at the danger: shallow and calm, strong current, or deep and dangerous.
- The ferry costs $20–50 and takes 3 days. The 3-day wait is now shown in the offer.
- Local help costs 3–8 hats.
- **Fixed:** coming back from an event could restart a river crossing. Each location now greets the party only once.

## Hunting
- **Fixed:** ammo use per shot was broken. Each shot now uses 1–3 ammo.
- The hunt ends when you run out of ammo.
- **Fixed:** the number of robots was re-rolled on every loop step. Each hunt now has 2–8 robots.
- Shots have a 20% chance to miss even when they're fast enough. Snipers miss only 5% of the time.
- Robot bounties are lower:

  | Robot | Bounty |
  |---|---|
  | Heavy | $50–120 |
  | Soldier | $30–80 |
  | Pyro, Demoman | $30–70 |
  | Medic | $30–60 |
  | Scout, Engineer, Sniper, Spy | $20–50 |

## Store
- **Fixed:** the purchase limit ignored what you were already carrying, so you could buy more than the maximum.
- The store shows how much food your team eats per day and where the next store is.
- The welcome text now explains how much food a team of four needs, how many hats everyone needs and what ammo is for.

## Game flow
- The game ends as soon as everyone has died, including mid-trip.
- **Fixed:** the victory screen could appear before you actually arrived at the last location.
- **Fixed:** the end-of-trail check in the trail module was off by one.
- The victory screen now shows the number of days taken, which members survived and a closing line based on how many made it.
