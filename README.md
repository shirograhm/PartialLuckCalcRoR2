# PartialLuckPlugin - RoR2
A library/dependency for [TooManyItems](https://thunderstore.io/package/shirograhm/TooManyItems/). Adds the ability for survivors to have partial luck.  
Supports integration with [LookingGlass](https://thunderstore.io/package/DropPod/LookingGlass/).  

### Changes
In the base game, luck is stored as a float, but is rounded up to the next integer value when calculating chance effects. This is because the base game simulates each roll using the luck value as a counter.  
This mod replaces that functionality with a continuous luck function instead.  

### Math
#### Positive Luck
Let's say you have **1 luck** and you want to proc an effect with a **30% chance** *(0.3)*.  
With 1 luck, you get 1 extra roll, so in order to successfully proc, you would have to "not fail" both rolls.  
`chance of failing ⇒ 1 - 0.3 ⇒ 0.7`  
`chance of failing twice ⇒ 0.7 * 0.7 ⇒ 0.49`  
`chance of success ⇒ 1 - (chance of failure) ⇒ 1 - 0.49 ⇒ 0.51 ⇒ 51% chance`  
The equation below is what follows for continuous positive values of luck.  
`chance of success = 1 - (1 - base chance)^(positive luck + 1)`  
#### Negative Luck
With **-1 luck**, we can follow a similar train of thought: you would have to succeed both rolls.  
`chance of success twice ⇒ 0.3 * 0.3 ⇒ 0.09`  
The equation below is what follows for continuous negative values of luck.  
`chance of success = (base chance)^(abs(negative luck) + 1)`  
#### Partial Luck
Now lets say you have **0.5 luck**:  
`chance of success ⇒ 1 - (1 - 0.3)^(0.5 + 1) ⇒ 1 - (0.7)^(1.5) ⇒ 1 - 0.586 ⇒ 0.414 ⇒ 41.4% chance`  
And similarly, with **-0.5 luck**:  
`chance of success ⇒ (0.3)^(abs(-0.5) + 1) ⇒ (0.3)^(1.5) ⇒ .14 ⇒ 16.4% chance`  
###### Example Scaling Table
<table>
  <tr>
    <th>Luck</th>
    <td>-1</th>
    <td>-0.5</th>
    <td>0</th>
    <td>0.5</th>
    <td>1</th>
  </tr>
  <tr>
    <th>Chance</td>
    <td>9%</td>
    <td>16.4%</td>
    <td>30%</td>
    <td>41.4%</td>
    <td>51%</td>
  </tr>
</table>

*TL;DR: Math hocus pocus allows non-integer luck values.*  

### Mod Compatibility
If you want to add partial luck to items, characters, etc. you can use this mod to do so. Register it as a dependency for your mod, then when you want to update someone's luck value, use the following code snippet:  
>`RecalculateStatsAPI.GetStatCoefficients += (sender, args) => {`  
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`...`  
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`PartialLuckTracker tracker = sender.master.gameObject.GetComponent<PartialLuckTracker>();`  
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`tracker.PartialLuck += [your partial luck value here];`  
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`...`  
>`}`  
