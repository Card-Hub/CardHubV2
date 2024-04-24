<script setup lang="ts">
import PlayerHand from "~/components/PlayerHand.vue";
import { ref, computed } from 'vue';
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

import { storeToRefs } from "pinia";
import { useWebSocketStore } from "~/stores/webSocketStore";

const store = useWebSocketStore();
const { connection, isConnected, messages, user, room } = storeToRefs(store);
const { tryCreateRoom, tryJoinRoom, sendGameType } = store;

const connectGameboard = async (): Promise<void> => {
  const isRoomCreated = await tryCreateRoom();
  if (isRoomCreated) {
    sendGameType('Blackjack');
    // await navigateTo('/playerview');
    await navigateTo("/lobby");
  }
};

// create standard deck of cards
const standardDeck = [];
const suits = ["hearts", "diamonds", "clubs", "spades"];
const values = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"];

for (const suit of suits) {
  for (const value of values) {
    standardDeck.push({
      id: standardDeck.length + 1,
      suit,
      value
    });
  }
}

const playerHand = ref<StandardCard[]>(standardDeck);


const showCards = ref(false);
const buttonText = ref('Show Cards');

const showCardContainer = () => {
  showCards.value = !showCards.value;
  buttonText.value = showCards.value ? 'Hide Cards' : 'Show Cards';

};

const getCards = () => {
  return new URL(`../../assets/icons/standardDeck/spades.svg`, import.meta.url);
};
</script>

<template>
  <div class="blackjack">
    <NuxtLink href="/games" class="go-back-btn">
      <Button class="go-back">Go Back</Button>
    </NuxtLink>

    <div class="column-container">
      <div class="column left-column">
        <div class="deck-view">
          <div class="flex justify-center items-center w-52 h-80 bg-white rounded-md shadow-md mb-2">
            <img :src="getCards()"
                 alt="game icon"
                 class="deck-logo"/>
          </div>
        </div>
      </div>

      <div class="column right-column">
        <h1 class="text-7xl">Blackjack</h1>
        <h3>Game Description: </h3>
        <p> In this classic card game, your goal is to beat the dealer's hand without going over 21. You'll be dealt two cards initially and have the option to 'hit' to receive another card or 'stand' to keep your current hand. The dealer will also be dealt cards, one face up and one face down. Be careful though - if your hand total exceeds 21, you'll 'bust' and lose the round. But if you manage to get closer to 21 than the dealer without busting, you win! Get ready to test your luck and skills in this exciting game of blackjack! </p>
        <NuxtLink href="/lobby">
          <Button class="play" label="Secondary" severity="secondary" @click="connectGameboard"> Play Blackjack </Button>
        </NuxtLink>
      </div>

      <!--      <div class="column">-->
      <!--        <h1>UNO</h1>-->
      <!--      </div>-->
    </div>
    <Button @click='showCardContainer()' class="show-cards">{{ buttonText }}</Button>

    <div v-if="showCards" class="card-container">
      <div v-if="showCards" class="card-container">
        <StandardCardDisplay v-for="card in standardDeck"
                        :key="card.id"
                        :card="card"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
.blackjack {
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 100%;
  width: 100%;

}

.deck-view {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding-top: 50px;
  height: 100%;
  width: 100%;
}

.deck-logo {
  width: 75%;
  align-items: center;
}

.show-cards {
  margin-top: 20px;
  margin-bottom: 20px;
  align-self: center;
  color: white;
  background-color: transparent;
  border: 2px solid white;
  width: 130px;
  text-align: center;
}

.column-container {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  height: 100%;
  width: 100%;
}

.left-column {
  flex: 0.5;
  align-items: center;
}

.right-column {
  flex: 1.5;
}

.column {
  margin: 0 10px;
}

.go-back-btn {
  padding-left: 20px;
  padding-top: 20px;
  padding-bottom: 10px;
}

.go-back {
  background-color: transparent;
  color: white;
  border: 2px solid white;
}

.play {
  background: linear-gradient(20deg, #6e0000 0%, #ff0000 75%);
  color: white;
  border: 2px solid #151515;
  align-self: center;
}

.card-container {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-self: center;
  flex-wrap: wrap;
  align-items: center;
  height: 100%;
  width: 85%;
}
</style>