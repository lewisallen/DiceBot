using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiceBot {
    public class DiceCommands {
        private readonly Random _random = new Random();

        [Command("r"), Description("randomly rolls dice")]
        public async Task RollDice(CommandContext ctx, [Description("dice to roll"), RemainingText] string str) {
            str = str.ToLower();
            while(str.Contains(" ")) str = str.Remove(str.LastIndexOf(' '), 1);
            var dice = str.Split('+');
            var total = 0;
            var tmp = "(";
            for (var k = 0; k < dice.Length; k++) {
                if (!dice[k].Contains("d")) {
                    total += int.Parse(dice[k]);
                    tmp += int.Parse(dice[k]) + "+";
                    break;
                }
            
                var die = dice[k].Split('d');  //lets say dice[1] = 1d10!>9 and means explode on 9 & 10, die[0]=1 & die[1]=10!>9.
                var exploding = die[1].EndsWith("!"); //var exploding = die[1].Contains("!"); As last char is 9 you have to check the whole string
                if (exploding) die[1] = die[1].Remove(die[1].LastIndexOf("!"));  //leaves you with 10>9
                var ammt = int.Parse(die[0]);
                /*
                var lowest=0;
                var doSplit=false;
                if(die[1].Contains(">")){  //if we need to split it up again. i.e. input was just 1d10! or 1d10!>9
                    doSplit=true;
                }
                var split=die[1].Split(">");
                var sides=0;
                if(doSplit){
                    lowest=int.Parse(split[1]);
                    sides=int.Parse(split[0]);
                }else{
                    sides = int.Parse(die[1]);  //replaces line 44
                }
                */
                var sides = int.Parse(die[1]);

                tmp += "(";
                for (var j = 0; j < ammt; j++) {
                    var next = _random.Next(1, sides+1);
                    total += next;
                    tmp += next + "+";
                    var nest = 1;
                    while (next == sides && exploding) {
                        next = _random.Next(1, sides+1);
                        total += next;
                        tmp += next + "+";
                        nest++;
                        if (nest == 32) {
                            break;
                        }
                    }
                }
                
                tmp = tmp.Remove(tmp.LastIndexOf('+'));
                tmp += ")+";
            }

            tmp = tmp.Remove(tmp.LastIndexOf('+'));
            tmp += ")";

            await ctx.RespondAsync(ctx.Member.Mention + " " + Formatter.BlockCode(str + "=" + tmp + "=" + total));
        }
    }
}
