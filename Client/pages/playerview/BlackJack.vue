<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
import {storeToRefs} from "pinia";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";
import StandardnoshadowCard from "~/components/noShadowCard/StandardnoshadowCard.vue";
import { useBlackJackStore } from "~/stores/blackJackStore";
import { useBaseStore } from "#imports";
// fullscreen
const { isFullscreen, enter, exit } = useFullscreen();
const el = ref(null)
const { toggle } = useFullscreen(el)

const store = useBlackJackStore();
const baseStore = useBaseStore();
const { user } = storeToRefs(baseStore);

const { drawBlackJackCard, standBlackJackPlayer, betBlackJackPlayer } = store;
const {players, currentPlayer, allPlayersHaveBet} = storeToRefs(store);
// const player = findPlayerByName(user);

const getPrimeIcon = (name: string) => {
  return new URL(`../../assets/icons/primeIcons/${name}.svg`, import.meta.url);
}

// https://primevue.org/dialog
import Dialog from 'primevue/dialog';
import Chat from "~/components/Chat.vue";
import UnoRules from "~/components/gameRules/UneRules.vue";
import PokerRules from "~/components/gameRules/PokerRules.vue";
import BlackjackRules from "~/components/gameRules/BlackjackRules.vue"; // for popup dialog
const rulesVisible = ref(false); // for popup dialog
const chatVisible = ref(false); // for popup dialog https://primevue.org/avatar/ for chat notification
const showingBetPopup = ref< boolean>(false);
const amtToBet = ref<number>(0);


const showBetPopup = () => {
  showingBetPopup.value = true;
}
const closeBetPopup = () => {
  showingBetPopup.value = false;
}

const bet = (amt: number) => {//invoke bet here
  closeBetPopup();
  betBlackJackPlayer(amt);
}

const hit = () => {//invoke drawcard function here
  drawBlackJackCard();
}

const stand = () => {
  standBlackJackPlayer();
}

const getUserIcon = () => {
  return new URL(`../../assets/icons/avatars/lyssie.png`, import.meta.url);
};

const findPlayerByName = (userConn: string) => {
      return players.value.find(player => player.Name === user.value);
};

const findPlayerByID = (userConn: string) => {
      return players.value.find(player => player.strConn === userConn);
};

</script>
 
<template>
  <p class="text-white"> fg{{  findPlayerByName(user)  }}</p>
  <p class="text-white"> yeye{{players}}</p>
  <div class="playerview-une-container w-full p-6">
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
            <BlackjackRules />
            <div class="flex justify-content-end gap-2">
            </div>
          </Dialog>
        </div>

        <div class="">
          <i class="pi pi-fw pi-comment" style="font-size: 2rem" @click="chatVisible = true"></i>
          <Dialog v-model="chatVisible" class="w-[900px] h-[900px]" header="Chat" :visible="chatVisible" @update:visible="chatVisible = $event">
            <Chat/>
          </Dialog>
        </div>

      </div>
    </div>all
    
    <div class="w-full margin-auto">
      <h1 class="text-center">
        <!-- will need to change backend to also have currentplayerConn-->
        <span v-if="currentPlayer === user && findPlayerByName(user)?.Winner == false && findPlayerByName(user)?.Busted == false && findPlayerByName(user)?.StillPlaying == true && findPlayerByName(user)?.Standing == false && findPlayerByName(user)?.NotPlaying == false && allPlayersHaveBet == true">
            Your Turn!
        </span>
        <span v-if="findPlayerByName(user)?.Winner == true">
            You Won!
        </span>
        <span v-if="findPlayerByName(user)?.Busted == true">
            You Lose!
        </span>
        <span v-if="currentPlayer !== user &&  allPlayersHaveBet === true">{{ findPlayerByID(user) }} is playing...
        </span>
        <span v-if="allPlayersHaveBet === false">
            Everyone Take Bets!
        </span>
      </h1>
    </div>

    <div class="big-container">

      <div class="top-row-container">
        <div>
          
        </div>
    
        <div class="cards-outer">
            <!--div class="cards">
              <StandardnoshadowCard :card="findPlayerByName('Dealer')?.Hand[0]" class="standardCardDisplay"/>
              <StandardnoshadowCard :card="findPlayerByName('Dealer')?.Hand[1]" class="standardCardDisplay"/>  
            </div-->
        </div>

        <div class="content-center stat-box">
            <ul class="">
              <li>
                Your current bet: {{findPlayerByName(user)?.CurrentBet}}
              </li>
              <li>
                Your remaining money: {{findPlayerByName(user)?.TotalMoney}}
              </li>
              <li>
                Your Cards Value: {{findPlayerByName(user)?.CurrentScore}}
              </li>
            </ul>
        </div>
      </div>

      <div class="buttons">
        <Button class="font-bold button shadow" @click="showBetPopup" :disabled="findPlayerByName(user)?.HasBet">Bet</Button><!--will just invoke function-->
        <Button class="font-bold button shadow" @click="hit" :disabled="currentPlayer != user || allPlayersHaveBet == false || findPlayerByName(user)?.Standing || !findPlayerByName(user)?.HasBet">Hit</Button><!--will just invoke function-->
        <Button class="font-bold button shadow" @click="stand" :disabled="currentPlayer != user || allPlayersHaveBet == false || findPlayerByName(user)?.Standing || !findPlayerByName(user)?.HasBet">Stand</Button><!--will just invoke function-->
      </div>
<!--      <div class="footer">-->
<!--      </div>-->
    </div>

  <div v-if="showingBetPopup" class="select-color">
    <h2>Select amount to bet</h2>
      <InputNumber v-model="amtToBet" inputId="horizontal-buttons" showButtons buttonLayout="horizontal" :step="1" class="">
        <template #incrementbuttonicon>
            <span class="pi pi-plus" />
        </template>
        <template #decrementbuttonicon>
            <span class="pi pi-minus" />
        </template>
      </InputNumber>
      <Button class="font-bold button raise-button shadow mt-2" @click="bet(amtToBet)">Bet</Button>
  </div>
</div>
  
  
</template>

<style scoped>

.top-row-container {
  display: grid;
  grid-template-columns: 1fr 2fr 1fr;
}

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

.buttons-container {
  display: flex;
  justify-content: center;
  align-items: flex-end;
  height: 100%;
}

.cards {
  /*background-color: green;*/
  display: grid;
  grid-template-columns: 1fr 1fr;
  padding-top: 20%;
}
.standardCardDisplay {
  /*background-color: pink;*/
  height: 100%;
}

.big-container {
  display: grid;
  /* grid-template-columns: 1fr 1fr 1fr; */
  height: 100vh;
  grid-template-rows: 1fr 1fr;
}

.buttons {
  display: grid;
  margin: auto;
  grid-template-columns: 1fr 1fr 1fr;
  grid-template-rows: 1fr;
  grid-gap: 5%;
  width: 60%;
}

.button {
  background-color: var(--cardhub-red);
  width: 100%;
  height: 95%;
  justify-content: center;
  color: black;
  font-size: 2.3em;
}

.stat-box {
  margin-right: 150px;
  margin-top: 120px;
  border-radius: 20px;
  background-color: var(--cardhub-red);
  width: 90%;
  height: 75%;
  justify-content: center;
  color: black;
  font-size: 2.3em;
}


.raise-button {
  height: auto;
}
.button:disabled {
  background-color: grey;
}

.footer {
  /*background-color: yellow;*/
  grid-template-columns: 2fr 3fr;
  display: grid;
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
  background-color: #745609;
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

.user-info {
  color: white;
  padding-left: 2px;
  padding-right: 10px;
  border-radius: 25%/50%;
}

.user-avatar {
  width: 3rem;
  height: 3rem;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.2);
}

</style>