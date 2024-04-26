<script setup lang="ts">
const icons = {
  skip: new URL(`../../assets/icons/unoDeck/skip.svg`, import.meta.url),
  reverse: new URL(`../../assets/icons/unoDeck/reverse.svg`, import.meta.url),
  drawTwo: new URL(`../../assets/icons/unoDeck/plus_two.svg`, import.meta.url),
  wild: new URL(`../../assets/icons/unoDeck/wild_circle.svg`, import.meta.url),
  wildDrawFour: new URL(`../../assets/icons/unoDeck/plus_four.svg`, import.meta.url),
  skipAll: new URL(`../../assets/icons/unoDeck/skip_all.svg`, import.meta.url),
};
</script>

<template>
  <div class="game-rules-uno-container p-6">
    <h1 class="text-center">UNE Rules</h1>
    <div class="rules">
      <h2>Objective</h2>
      <p>The objective of UNE is to be the first player to get rid of all the cards in their hand. Players must match the card in the discard pile by either color, number, or symbol. If a player has no matching card, they must draw a card from the draw pile. THen, the play moves to the next person.</p>
<!--      If that card can be played, they may play it. Otherwise,-->
      <h2>Setup</h2>
      <p>UNE is played with a deck of 118 cards. The deck consists of cards of 4 colors: red, yellow, green, and blue. Each color has cards numbered 0-9, as well as action cards. The action cards include Skip, Skip All, Reverse, and Draw Two. There are also Wild and Wild Draw Four cards.</p>
      <h2>Gameplay</h2>
      <p>At the beginning of the game, each player is dealt 7 cards. The remaining cards are placed in a draw pile, with the top card placed face up next to it. On a player's turn, they must play a card that matches the color, number, or symbol of the top card in the discard pile. If a player has no matching card, they must draw a card from the draw pile. Play moves to the next person.</p>
      <h2>Action Cards</h2>
      <p>Action cards have special effects that can change the course of the game. </p>
      <div class="action-card">
        <img class="action-logo" alt="action-logo" :src="icons.skip"> <p> Skip cards cause the next player to lose their turn.</p> 
      </div>
      <div class="action-card">
        <img class="action-logo" alt="action-logo" :src="icons.reverse"> <p> Reverse cards reverse the direction of play. </p> 
      </div>
      <div class="action-card">
        <img class="action-logo" alt="action-logo" :src="icons.drawTwo"> <p> Draw Two cards cause the next player to draw two cards. </p> 
      </div>
      <div class="action-card">
        <img class="action-logo" alt="action-logo" :src="icons.skipAll"> <p> Skip All cards cause all other players to lose their turn. </p>
      </div>
      <div class="action-card">
        <img class="action-logo" alt="action-logo" :src="icons.wild"> <p> Wild cards can be played on any card, and the player who plays it gets to choose the color that must be played next. </p>
      </div>
      <div class="action-card">
        <img class="action-logo" alt="action-logo" :src="icons.wildDrawFour"> <p>Wild Draw Four cards have the same effect as Wild cards, but the next player must draw four cards.</p>
      </div>
      <h2>Winning the Game</h2>
      <p>The first player to get rid of all the cards in their hand wins the game. If a player has one card left, they must click "UNE" to alert the other players. If they fail to do so and are caught by another player, they must draw two cards. If a player plays their next-to-last card and forgets to say "UNE," they are subject to the same penalty.</p>
    </div>
  </div>
</template>

<style scoped>
.game-rules-uno-container {
  width: 100%;
  height: 75%;
  margin: 0 auto;
}

.action-card {
  display: flex;
  align-items: center;
  margin: 1rem;
}

.action-logo {
  margin-right: 1rem;
}

</style>