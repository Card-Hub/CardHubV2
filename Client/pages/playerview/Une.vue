<script setup lang="ts">
  import {defineComponent, ref, onMounted, type ComputedRef, type Ref, computed} from "vue";
  import {storeToRefs} from "pinia";
  import {useWebSocketStore} from "~/stores/webSocketStore";
  import toast from "@/utils/toast";

  import { type ConfigurableDocument, type MaybeElementRef, useFullscreen } from '@vueuse/core';
  
  // fullscreen
  const { isFullscreen, enter, exit } = useFullscreen();
  const el = ref(null)
  const { toggle } = useFullscreen(el)

  const getPrimeIcon = (name: string) => {
    return new URL(`../../assets/icons/primeIcons/${name}.svg`, import.meta.url);
  }

  import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
  import UnoRules from "~/components/gameRules/UnoRules.vue";
  
  // https://primevue.org/dialog
  import Dialog from 'primevue/dialog';
  import Chat from "~/components/Chat.vue"; // for popup dialog
  const rulesVisible = ref(false); // for popup dialog
  const chatVisible = ref(false); // for popup dialog https://primevue.org/avatar/ for chat notification
  // const sideScroll= ref(false); // for scrolling view or all cards are viewable on screen by scrolling down

  const store = useWebSocketStore(); 
  const {user, users, room, connection } = storeToRefs(store);
  const { playCard, selectColor, drawCard, pressUne } = store;
  const uneStore = useUneStore();
  const { winner, currentPlayer, players, discardPile, someoneNeedsToSelectColor, playerWhoHasPrompt, currentColor } = storeToRefs(uneStore);
 interface Player {
         Name: string
         Avatar: string
         Afk: boolean
     };
     
     interface unePlayer extends Player {
         PickingWildColor: boolean
         Hand: UNOCard[]
         CanPressUne: boolean
     };

  const myCards = computed(() => {
  let mc = <UNOCard[]>[];
  if (players.value != null) {
    players.value.forEach(player => {
      if (player.Name == user.value) {
        player.Hand.forEach(card => {
          // translate between json and unocard
          mc.push({Id:card.Id, Value:card.Value, Color:card.Color});
        });
      }
    });
  }
  // sort cards by color then value
  mc.sort((a, b) => {
    if (a.Color < b.Color) {
      return -1;
    } else if (a.Color > b.Color) {
      return 1;
    } else {
      if (a.Value < b.Value) {
        return -1;
      } else if (a.Value > b.Value) {
        return 1;
      } else {
        return 0;
      }
    }
  });
  return mc;
});
// object of players with information about avatar, name, and cards

  
  const callUne = () => {
    // record who pressed button first
    playerWhoHasPrompt.value = user.value;
    
    // disable une button for all players
    document.getElementById("uneButton").setAttribute("disabled", "disabled");
    
    // check if current player was the first to press the button and give them a card if they were not
    if (playerWhoHasPrompt.value !== currentPlayer.value) {
      drawCard();
      drawCard();
    }
    playerWhoHasPrompt.value = ""; // reset prompt
    
  };
  
  // enable une button if player has one card left
  const validateUneCall = () => {
    let isValid = false;
    players.value.forEach(player => {
      if (player.Hand.length < 3) {
        if (player.Name === currentPlayer.value) {
          isValid = true;
        }
      }
    });
    return isValid;
  };

  const canPressUne = () => {
    let canPress = false;
    players.value.forEach(player => {
      if (player.Name === user.value && player.CanPressUne) {
        canPress = true;
      }
    });
    return canPress;
  }
  
  const getWinnerIcon = (player: string) => {
    // iterate through players to find the winner's avatar
    let winnerIcon = "";
    players.value.forEach(p => { if (p.Name == player) { winnerIcon = p.Avatar; } });
    
    return new URL(`../../assets/icons/avatars/${winnerIcon}.png`, import.meta.url);
  };
  
  const getUserIcon = () => {
    // iterate through players to find the user's avatar
    let userIcon = "";
    players.value.forEach(p => { if (p.Name === user.value) { userIcon = p.Avatar; } });
    
    return new URL(`../../assets/icons/avatars/${userIcon}.png`, import.meta.url);
  };
  
  const handleExit = async () => {
    // store.leaveRoom();
    connection.value = null;
    room.value = '';

    // redirect to join page
    await navigateTo("/join");
  };
  
  const isCurrentPlayer = () => {
    if (user.value !== currentPlayer.value) {
      toast.add({
        severity: "error",
        summary: "It's not your turn!",
        detail: "Please wait for your turn to play!",
        life: 5000
      });
    }
    
  };

  const canBePlayed = (card: UNOCard) => {
    return card.Color.toLowerCase() === currentColor.value.toLowerCase() || card.Value.toLowerCase() === discardPile.value[discardPile.value.length - 1].Value.toLowerCase() || card.Color.toLowerCase() === 'black';
  };

</script>


<template>
  <div class="playerview-une-container w-full p-6">
    <Toast/>
    
    <div class="flex flex-row justify-between">
      <div class="user-info flex place-items-center gap-3 bg-gray-600">
          <img :src="getUserIcon()" alt="user" class="user-avatar"/>
          <p class="text-white"> {{ user }}</p>
      </div>
      
      <div class="left-div flex flex-row-reverse place-items-center gap-2">
        <img :src="getPrimeIcon('expand')" class="size-10" @click="toggle" />

        <div class="card">
          <i class="pi pi-fw pi-info-circle" style="font-size: 2rem" @click="rulesVisible = true"></i>
          <Dialog v-model="rulesVisible" header="Rules" class="w-[900px] h-[900px]" :visible="rulesVisible" @update:visible="rulesVisible = $event">
            <UnoRules/>
            <div class="flex justify-content-end gap-2">
              <!--            <Button type="button" label="Exit" @click="rulesVisible = false"></Button>-->
            </div>
          </Dialog>
        </div>

        <div class="">
          <i class="pi pi-fw pi-comment" style="font-size: 2rem" @click="chatVisible = true"></i>
          <Dialog v-model="chatVisible" class="w-[900px] h-[900px]" header="Chat" :visible="chatVisible" @update:visible="chatVisible = $event">
            <Chat/>
          </Dialog>
        </div>

        <!--      <i class="pi pi-fw pi-eye" @click="!sideScroll" style="font-size: 2.5rem"></i>-->
      </div>
    </div>
    
    
    <div class="w-full margin-auto">
      <h1 class="text-center">
        <span v-if="currentPlayer === user && winner != user">
            Your Turn!
        </span>
        <span v-if="winner == user">
            You Won!
        </span>
        <span v-if="currentPlayer !== user && winner === ''">{{ currentPlayer }} is playing...
        </span>
        <span v-if="currentPlayer !== user && winner !== ''">
          {{ winner }} won!
        </span>
      </h1>
    </div>
    
    <Button v-if="winner===''" class="float-right font-bold drawButton shadow mb-4" @click="drawCard">Draw</Button>
    <Button v-if="winner===''" class="float-left font-bold uneButton shadow mb-4" :disabled="!canPressUne()" @click="pressUne">UNE!</Button>
    
    <div v-if="winner===''" class=" w-full flex flex-wrap justify-center">
      <UNOCardDisplay class="uneCard flex-wrap"
                      v-for="card in myCards"
                      :key="card.Id"
                      :card="card"
                      :isSelected="canBePlayed(card) && currentPlayer === user"
                      @click="{...playCard(JSON.stringify(card)), ...isCurrentPlayer()}"
      />
    </div>

<!--    <div v-if="winner==='' && updateScroll ===true" class=" w-full flex overflow-x-auto border-2 border-radius-4 justify-center">-->
<!--      <UNOCardDisplay class="uneCard flex-wrap"-->
<!--                      v-for="card in myCards"-->
<!--                      :key="card.Id"-->
<!--                      :card="card"-->
<!--                      :isSelected="(card.Color.toLowerCase() === discardPile[discardPile.length - 1].Color.toLowerCase() || card.Value === discardPile[discardPile.length - 1].Value || card.Color.toLowerCase() === 'black') && currentPlayer === user"-->
<!--                      @click="playCard(JSON.stringify(card))"-->
<!--      />-->
<!--    </div>-->
    
    <div v-if="currentPlayer === user && someoneNeedsToSelectColor">
    </div>
    
    <!--  winner -->
    <div v-if="winner!=''" class="">
      <div class="winner">
        <div class="winner-inner">
          <span v-if="winner == user" class="flex flex-col">
            <img :src="getWinnerIcon(user)" alt="winner" class="avatar-inner"/>
            You Won!
          </span>
            <span v-if="currentPlayer !== user && winner !== ''" class="flex flex-col">
            <img :src="getWinnerIcon(winner)" alt="winner" class="avatar-inner"/>
            {{ winner }} won!
          </span>
        </div>
      </div>
      <Button class="exit-btn" @click="handleExit()"> Exit </Button>
    </div>
    
    <div v-if="winner==='' && someoneNeedsToSelectColor && currentPlayer === user && someoneNeedsToSelectColor" class="select-color">
      <h2>Select a Color</h2>
      <div class="select-color-inner">
        <Button class="red-button color-button" @click="selectColor('red')">Red</Button>
        <Button class="green-button color-button" @click="selectColor('green')">Green</Button>
        <Button class="blue-button color-button" @click="selectColor('blue')">Blue</Button>
        <Button class="yellow-button color-button" @click="selectColor('yellow')">Yellow</Button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.cards-container {
  border-color: red;
  /*display: flex;*/
  /*flex-direction: row-reverse;*/
}
.playerview-une-container {
  width: 100%;
  height: 100vh;
  /*display: flex;*/
  justify-content: center;
  /*align-items: center;*/
  background: rgb(243, 19, 19);
  background: radial-gradient(ellipse at center, rgba(243, 19, 19, 0.5) 0%, rgba(152, 14, 17, 0.7) 40%, rgba(63, 8, 14, 0.9) 95%);
}

.drawButton {
  background-color: var(--cardhub-red);
  width: 15%;
  height: 8%;
  justify-content: center;
  color: white;
  font-size: 2em;
}

.shadow {
  box-shadow: 4px -4px 6px rgba(256, 256, 256, 0.15);
}
.winner {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
  transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: rgba(243, 19, 19, 0.5);
  width: 80%;
  height: 25%;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  /*justify-content: center;*/
  border-radius:4px;
  border-style: solid;
  border-width: 2px;
  padding: 10px;

}
.winner-inner {
  border-style: solid;
  border-width: 4px;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center; /* center*/
  align-items: center; /*height*/
  flex-direction: column;
}

.select-color {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
  transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: rgb(63, 8, 14);
  width: 40%;
  height: 50%;
  /*display: flex;*/
  /*flex-direction: column;*/
  /*align-items: center;*/
  /*justify-content: center;*/
  text-align: center;
  justify-content: center;
  border-radius:4px;
  border-style: solid;
  border-width: 2px;
  padding: 12px;
  border-color: var(--cardhub-red);
}
.select-color h2 {
  margin-top: 0;
}
.select-color-inner {
  display:grid;
  height: 80%;
  grid-template-columns: 48% 48%;
  grid-template-rows: auto auto;
  column-gap: 4%;
  row-gap: 6%;
  /*row-gap: 4%;*/
  /*grid-row: 48% 48%;*/
}

.red-button {
  background-color: var(--une-red);
}
.blue-button {
  background-color: var(--une-blue);
}
.green-button {
  background-color: var(--une-green);
}
.yellow-button {
  background-color: var(--une-yellow);
}
.color-button {
  font-weight: bold;
  font-size: 1.5em;
  text-align: center;
  justify-content: center;
}

.uneButton {
  background-color: var(--cardhub-red);
  width: 15%;
  height: 8%;
  justify-content: center;
  color: white;
  font-size: 2em;
}

.chat-container {
  width: 80%;
  height: 80%;
  max-height: 400px;
}

.avatar-inner {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-right: 10px;
  background: rgba(255, 255, 255, 0.2);
  margin-bottom: 10px;
}

.user-avatar {
  width: 3rem;
  height: 3rem;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.2);
}

.exit-btn {
  background-color: transparent;
  width: 15%;
  height: 5%;
  justify-content: center;
  color: white;
  font-size: 1.5em;
  border: 2px solid white;
  transform: translateY(-50%) translateX(-50%);
  position: absolute;
  top: 70%;
  left: 50%;
}

.user-info {
  color: white;
  padding-left: 2px;
  padding-right: 10px;
  border-radius: 25%/50%;
}
</style>