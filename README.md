# TerrariumRpg

### Overview
- Main character is a guy who's a "General" scrying through a pool to let characters go out and adventure, his name is Dufyt.
- Every "day" adventurers are trained and available from the town to send out and adventure
- Characters can permanently die in pretty easy ways, and those characters get "retired".
- Items can be transferred between party members
- Need to be able to copy/paste subroutines between characters to make it easy
- Hub and spoke system for the world overview
- Seed the world generation so it's random to some extent. Allow for player to put their own seed in.

### Wishlist
- Easter eggs with Wolves or Bear army gathering in the background
- 

### Problems to solve
- How to categorize the interactions
    1. Recognize that there's a place/spatial awareness. aka pathfinder/A-Star. Something that can identify different tile types as a thing so it can make decisions about something in front of it.
        - Categorize a tile as things like trees, road, field, town, npc, monster, dungeon, mountain, impassible terrain, etc.
        - For the decision tree, users might need to be able to choose what action to do when they see specific tile types
        - Have some base behavior that all PC types would have, but which allows for a player to override the behavior if they wanted to when they see a certain tile.
    1. Early part of the game needs to teach players the system slowly, so it's in a controlled way.
        - We should tie this to the player, not necessarily their character, so if they have a new character they don't have to redo the whole thing.
        - Some progression mechanics are linked to the PC, some are linked to the player themselves.
-  We could have death just bring you back to the previous save point/checkpoint, and the player can choose different behavior based on whatever it was that got them killed.
    - We could have a certain number of re-tries that they get before the character dies in a more substantial way.
    - Represent the before mission state and the after mission state with all the character level, items, etc. to see if it was good or not.
    - Need to have an audit trail/log of all the things that happened between two points in time the player can review and see what happened after

### Combat mechanics
- Combat is resolved instantly, and there's a high level review
- Melee attacks can only be done on N/S/E/W tiles, ranged can happen from any tile as long as they have LoS
- We don't expose specific mechanics in the combat
    - e.g. no
        - "Tyfud rolls a 10 + 6 for 16 total and the attack hits"
    - Yes
        - "Tyfud won the combat and was hit 3 times for 55 damage"
        - "Tyfud was killed by a group of 2 Goblins after 5 rounds of combat. He took 20 points of poison damage, and 50 points of physical damage"
- Show, don't tell
- Have significant combat events shown on the screen as it's progressing
- Show combat on the main screen, with monsters taking specific tiles, and your character killing/fighting them for each round of the combat
- Brouge game has a very similar mechanic to what we want called "Explore" that we want our game to do.
- #### Math/Mechanics
    - 3D6 system with a "10" needed for a success
    - 4D6 for Advantage
    - 2D6 for Disadvantage
    - Crit on 18+
    - Roll to hit, no confirmation needed if you hit
    - There is nothing that can increase your dice rolls here, you can only have advantage or disadvantage
    - You get a D"X" for every level above something you are on your 3D6 roll, up to 6 levels which shifts you to Advantaged at 4D6. Same for levels above you, you receive a -D"X" for each level. 6 levels +/- of the character for 3D6 (normal), if more than 5 levels above something, you have advantage, if it's more than 5 levels above you, you have disadvantage. The cap is a +/- D6 at the top and bottm ends. Examples:
        - Character is 3 levels above an NPC, he rolls a 3D6 + 1D3 for the roll. 3D6 roll = 8, D3 = 2, total = 10, which is a success
        - Character is 5 levles below an NPC, he rolls a 3D6 - 1D5 for the roll. 3D6 roll = 13, D5 = 4, total = 9, which is a failure
        - Character is 15 levels below an NPC, he rolls a 3D6 - 1D6 (2D6 total for disadvantage) for the roll. 2D6 roll = 7, which is a failure
    - Formula for to hit: `3D6 + 1D(Clamp((CharacterLevel - NPC Level), -6, 6))`
    - Attack value:
        - Weapon, character base, etc.
        - If your atta
    - Defense value:
    - ##### Damage
        - There's no rolls for damage
        - If your attack value is higher than their defense value, you do your attack value in damage. If their defense is equal or greater than your attack, no damage is dealt
    - ##### Stats
        - HP = Wounds.
            - NPCS: When wounds are depleted they die
            - PCs: When reaching 0, receives an injury
                - Each injury increases their chance of permanently dying
                - Some rare items or abilities can remove injuries
                - If character dies they're permanently dead/(unable to be sent back out)
                - If whole party becomes injured they return to town
                - You can always have your characters come back, but you may not be able to send them back out. You can re-use their gear, but on another character
                
### Character advancement
- Based on how much gold you bring back after completing the adventure
- Gold goes into the community coffers which your General can spend
- Gold/treasure value is split equally between each of the party members
- If you abandon the quest, you keep what you've found so far, they can return to town, but you don't get the quest bonus stuff or multipliers

### Explore mechanics
- We need an objective/cheese for each mission/quest so the Explore feature can determine if there's a success/how to move forward and when explore finishes
- If no cheese/objective is found, then they return back to town
- Each mechanic would have its own subroutine that the user would expand/modify
- Quest mechanic subroutines that are ephemeral just for the quest itself, and does not carry/stick with the PC characters
- Have a way to prioritize different subroutines as what we would do first on the player
- Need minimap to show where PC/Party is in the overworld map
- Different subroutine types:
    - Quest/Objective:
        - Parts of it you can't edit
        - What win/lose conditions are
        - Ephemeral and disappear after the quest
    - Movement:
        - Pathfinding
        - Discovery
            - NPCs
        - Overworld
    - Combat:
        - Adjacent combat
        - Line of sight combat
    
- #### Explore Loop mechanics
    - Keep track of the actions/decision tree behavior for each decision made for each log output entry.
        - This will capture the current actions and associate it to the log so you can review what the decision tree info was for each log item captured out
        - Gives the ability to review NPC AI decision tree behavior as well

### Codex
- You have a new mob/npc show in the codex the first time you encounter it
- You can't change your character/quest logic during a quest

### Town
- Safe base to return to that anchors the player when a quest succeeds/fails
- Access to place to sell, gear, quests, etc.
- Returning to town is a Rogue-like reset for any debuffs, injuries, wounds, stats, etc.

### UI/UX
- All panel components are draggable and movable
- Party view/stat view
- Circular rendering bit
- Mini-map
- Log view with printout info
    - Log should be a stack of what actions/flow items were done
- Codex to show the workflow screen to show the subroutines and choices for party characters
- Highlight decision tree flows that fired at certain points within the log

### Phase 1 MVP
- Set up project behavior
- Set up host for the .NET REST API
- Set up ReactJS stuff
- Set up CICD
- Set up a database to start storing and represent character data (SQLite to start with, AzureDB later)
- Consider using Azure instead of AWS
- Two rooms
    - Starting room with one PC entity
    - Other room with one entity/NPC
    - Single mechanic, like a door or bridge to go between the two rooms
    - Runs a single check for what's in the other room to know if there's an NPC in there
    - No Combat
- Single action/Workflow item that a player can change for what to do when an NPC is discovered
    - Attack
    - Flee
- Simplistic UI
    - Representation of single Workflow item with two choices
    - Tile representation of the "world"
    - Two tile types:
        - Traversible
        - Impassible
    - Button for the "Explore" loop
    - Mouseover for tile description (including NPC/PC info)
    - Log Output for movement and decision tree output