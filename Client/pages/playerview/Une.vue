<script setup lang="ts">
  import {defineComponent, ref, onMounted} from "vue";
  import {storeToRefs} from "pinia";
  import {useWebSocketStore} from "~/stores/webSocketStore";
  
  import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
  import UnoRules from "~/components/gameRules/UnoRules.vue";
  
  // https://primevue.org/dialog
  import Dialog from 'primevue/dialog';
  import Chat from "~/components/Chat.vue"; // for popup dialog
  const rulesVisible = ref(false); // for popup dialog
  const chatVisible = ref(false); // for popup dialog

  const store = useWebSocketStore(); 
  const {user, users, room } = storeToRefs(store);
  const { playCard, selectColor, drawCard } = store;
  const uneStore = useUneStore();
  const { winner, currentPlayer, yourCards, players, discardPile, someoneNeedsToSelectColor } = storeToRefs(uneStore);
 interface Player {
         Name: string
         Avatar: string
         Afk: boolean
     };
     
     interface unePlayer extends Player {
         PickingWildColor: boolean
         Hand: UNOCard[]
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
    // store.callUne();
  };
  
  // enable une button if player has one card left
  const enableUne = computed(() => {
    return myCards.value.length >= 1;
  });
</script>


<template>
  <div class="playerview-une-container w-full p-6">
    
    <div class="flex flex-row-reverse">
      <div class="card flex justify-left">
        <i class="pi pi-fw pi-info-circle" style="font-size: 2rem" @click="rulesVisible = true"></i>
        <Dialog v-model="rulesVisible" header="Rules" :visible="rulesVisible" @update:visible="rulesVisible = $event">
          <UnoRules/>
          <div class="flex justify-content-end gap-2">
            <Button type="button" label="Exit" @click="rulesVisible = false"></Button>
          </div>
        </Dialog>
      </div>
      
      <div class="justify-left">
        <i class="pi pi-fw pi-comment" style="font-size: 2rem" @click="chatVisible = true"></i>
        <Dialog v-model="chatVisible" header="Chat" :visible="chatVisible" @update:visible="chatVisible = $event">
          <Chat/>
          <div class="flex justify-content-end gap-2">
            <!--              <Button type="button" class="exit-button" label="Exit" @click="visible = false"></Button>-->
          </div>
        </Dialog>
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
    
    <Button class="float-right font-bold drawButton shadow mb-4" @click="drawCard">Draw</Button>
    <Button class="float-left font-bold uneButton shadow mb-4" disabled @click="callUne">UNE!</Button>
    
    <div v-if="winner===''" class=" w-full flex overflow-x-auto border-2 border-solid border-[#960E16] border-radius-4 justify-center">
      <UNOCardDisplay class="uneCard flex-wrap"
                      v-for="card in myCards"
                      :key="card.Id"
                      :card="card"
                      :isSelected="false"
                      @click="playCard(JSON.stringify(card))"
      />
    </div>
    
    <div v-if="currentPlayer === user && someoneNeedsToSelectColor">
    </div>
    
    <!--  winner -->
    <div v-if="winner!=''" class="winner">
      <div class="winner-inner">
        <span v-if="winner == user">
          You Won!
        </span>
        <span v-if="currentPlayer !== user && winner !== ''">
          {{ winner }} won!
        </span>
      </div>
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
  height: 20%;
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
  /*text-align: center;*/
  align-items: center; /*height*/
}

.select-color {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: rgb(63, 8, 14);
  width: 40%;
  height: 30%;
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

</style>