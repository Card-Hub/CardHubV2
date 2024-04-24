<script setup lang="ts">
  import { ref, computed } from 'vue';
  import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";

  import { storeToRefs } from "pinia";
  import { useWebSocketStore } from "~/stores/webSocketStore";

  const store = useWebSocketStore();
  const { connection, isConnected, messages, user, room } = storeToRefs(store);
  const { tryCreateRoom, tryJoinRoom, sendGameType } = store;

  const connectGameboard = async (): Promise<void> => {
    const isRoomCreated = await tryCreateRoom();
    if (isRoomCreated) {
      await sendGameType("UNE");
      // await navigateTo('/playerview');
      await navigateTo("/lobby");
    }
  };


  // create uno deck of cards
  const unoDeck = [];
  const colors = ["#d12c15", "#ffce30", "#7abb18", "#1166ac"];
  const unoValues = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Skip", "Reverse", "Draw Two", "Skip All", "Skip", "Reverse", "Draw Two", "Skip All"];

  for (const color of colors) {
    for (const value of unoValues) {
      unoDeck.push({
        Id: unoDeck.length + 1,
        Color: color,
        Value: value
      });
    }
  }

  // push 4 wild cards and 4 draw 4 wild cards
  for (let i = 0; i < 4; i++) {
    unoDeck.push({
      Id: unoDeck.length + 1,
      Color: "#151515",
      Value: "Wild"
    });
    unoDeck.push({
      Id: unoDeck.length + 1,
      Color: "#151515",
      Value: "Wild Draw Four"
    });
  }

  const playerHand = ref<UNOCard[]>(unoDeck);
  
  
  const showCards = ref(false);
  const buttonText = ref('Show Cards');
  
  const showCardContainer = () => {
    showCards.value = !showCards.value;
    buttonText.value = showCards.value ? 'Hide Cards' : 'Show Cards';
    
  };
  
  const getUNE = () => {
    return new URL(`../../assets/icons/unoDeck/UNE.svg`, import.meta.url);
  };
</script>

<template>
  <div class="une">
    <NuxtLink href="/games" class="go-back-btn">
      <Button class="go-back">Go Back</Button>
    </NuxtLink>

    <div class="column-container">
      <div class="column left-column">
        <div class="deck-view">
          <div class="flex justify-center items-center w-52 h-80 bg-zinc-800 rounded-md shadow-md mb-2">
            <img :src="getUNE()"
                 alt="game icon"
                 class="une-logo"/>
          </div>
        </div>
      </div>
      
      <div class="column right-column">
        <h1 class="text-7xl">UNE</h1>
        <h3>Game Description: </h3>
        <p> Each player begins with a hand of 7 UNE cards. The goal of the game is to rid yourself of your cards as quickly and efficiently as possible. The only ways to win the game is by being the first player to run out of cards ro by having the least amount of cards at the end of the game. To do this, you will need to play cards from your hand to match the number, color, or the action of the top card in the discard pile. </p>
        <NuxtLink href="/lobby">
          <Button class="play" label="Secondary" severity="secondary" @click="connectGameboard"> Play UNE </Button>
        </NuxtLink>
      </div>
      
<!--      <div class="column">-->
<!--        <h1>UNO</h1>-->
<!--      </div>-->
    </div>
    <Button @click='showCardContainer()' class="show-cards">{{ buttonText }}</Button>
    <div v-if="showCards" class="card-container">
      <UNOCardDisplay v-for="card in unoDeck"
                      :key="card.Id"
                      :card="card"
      />
    </div>
    
  </div> 
</template>

<style scoped>
.une {
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

.une-logo {
  width: 200%;
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
  flex-wrap: wrap;
  align-self: center;
  align-items: center;
  height: 100%;
  width: 85%;
}
</style>