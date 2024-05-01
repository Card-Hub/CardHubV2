<script setup lang="ts">

</script>

<template>
  <div class="game-rules-poker-container">
    <h1 class="text-center">Poker</h1>
    
    <h2>Objective</h2>
    <p>The objective of Poker is to win chips by having the best hand or by bluffing other players into folding.</p>
    
    <h2>Setup</h2>
    <p>Poker is played with a standard deck of 52 cards. The deck consists of four suits: hearts, diamonds, clubs, and spades. Each suit has cards numbered 2-10, as well as face cards: Jack, Queen, King, and Ace.</p>
    
    <h2>Gameplay</h2>
    <p>At the beginning of the game, each player is dealt two cards. Players then take turns betting, raising, or folding. After the initial round of betting, three community cards are dealt face up on the table. Players then have the option to bet, raise, or fold again. Two more community cards are dealt, with a round of betting after each card. Players can use any combination of their two hole cards and the five community cards to make the best hand.</p>
    
    <h2>Hand Rankings</h2>
    <p>The hand rankings in Poker, from highest to lowest, are as follows:</p>
    <ul>
      <li>Royal Flush: A, K, Q, J, 10 of the same suit</li>
      <li>Straight Flush: Five consecutive cards of the same suit</li>
      <li>Four of a Kind: Four cards of the same rank</li>
      <li>Full House: Three of a Kind and a Pair</li>
      <li>Flush: Five cards of the same suit</li>
      <li>Straight: Five consecutive cards of any suit</li>
      <li>Three of a Kind: Three cards of the same rank</li>
      <li>Two Pair: Two pairs of cards</li>
      <li>One Pair: Two cards of the same rank</li>
      <li>High Card: The highest card in the hand</li>
    </ul>
    
    <h2>Winning the Game</h2>
    <p>The player with the best hand at the end of the final round of betting wins the pot. If all other players fold, the remaining player wins the pot without showing their hand. If two or more players have the same hand, the pot is split between them.</p>
    
    <h2>Key</h2>
    <i class="pi pi-user" style="color: green;  font-size: 4rem; box-shadow: 0 0 10px #00FF00"></i> = Current Button Player; the player currently designated as the dealer for the hand. <br><br>
    <i class="pi pi-user" style="color: red;  font-size: 4rem; box-shadow: 0 0 10px #D60E26"></i> = Current Player's Turn <br><br>
    <i class="pi pi-user" style="color: blue;  font-size: 4rem; box-shadow: 0 0 10px #0000FF"></i> = Current Big Blind; the player who sets the higher bet. <br><br>
    <i class="pi pi-user" style="color: yellow;  font-size: 4rem; box-shadow: 0 0 10px #FFFF00"></i> = Current Small Blind; the player who sets the minimum bet. <br><br><br><br>
  </div>
</template>

<style scoped>
.game-rules-poker-container {
  width: 100%;
  height: 75%;
  margin: 0 auto;
}

</style>