<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
//impor
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";
import StandardnoshadowCard from "~/components/noShadowCard/StandardnoshadowCard.vue";

interface Player {
    Name: string
    Avatar: string
    Afk: boolean
};
     
interface PokerPlayer extends Player {
    Hand: StandardCard[]
    CurrentScore: number
    TotalMoney: number
    CurrentBet: number
    HasBet: boolean
    NotPlaying: boolean
    Busted: boolean
    Winner: boolean
    StillPlaying: boolean
    Standing: boolean
};

const cards2 = ref<StandardCard[]>([ {Id: 3, Suit: "Spades", Value: "4"}, {Id: 3, Suit: "Spades", Value: "4"}]);
const HasBet = ref<boolean>(true);
const NotPlaying = ref<boolean>(false);
const Busted = ref<boolean>(false);
const Winner = ref<boolean>(false);
const StillPlaying = ref<boolean>(true);
const Standing = ref<boolean>(false);
const showingBetPopup = ref<boolean>(false);


const CurrentScore = ref<number>(8);
const CurrentBet = ref<number>(6);
const TotalMoney = ref<number>(80);
const amtToBet = ref<number>(0);
const winners = ref<String[]>
const losers = ref<String[]>
const user = ref<string>("Me");
const currentPlayer = ref<string>("Me");
const AllPlayersHaveBet = ref<boolean>(false)

const showBetPopup = () => {
  showingBetPopup.value = true;
}
const closeBetPopup = () => {
  showingBetPopup.value = false;
}

const bet = (amtToBet) => {//invoke bet here

}

const hit = () => {//invoke drawcard function here

}

const stand = () => {

}

</script>
 
<template>
  <div class="playerview-une-container w-full p-6">
    <div class="w-full margin-auto">
      <h1 class="text-center">
        <span v-if="currentPlayer === user && Winner == false && Busted == false && StillPlaying == true && Standing == false && NotPlaying == false && AllPlayersHaveBet == true">
            Your Turn!
        </span>
        <span v-if="Winner == true">
            You Won!
        </span>
        <span v-if="Busted == true">
            You Lose!
        </span>
        <span v-if="currentPlayer !== user">{{ currentPlayer }} is playing...
        </span>
        <span v-if="AllPlayersHaveBet == false">
            Everyone Take Bets!
        </span>
      </h1>
    </div>

    <div class="big-container">

      <div class="top-row-container">
        <div>
          
        </div>

        <div class="cards-outer">
            <div class="cards">
              <StandardnoshadowCard :card="cards2[0]" class="standardCardDisplay"/>
              <StandardnoshadowCard :card="cards2[1]" class="standardCardDisplay"/>  
            </div>
        </div>

        <div class="content-center stat-box">
            <ul class="">
              <li>
                Your current bet: {{CurrentBet}}
              </li>
              <li>
                Your remaining money: {{TotalMoney}}
              </li>
              <li>
                Your Cards Value: {{CurrentScore}}
              </li>
            </ul>
        </div>
      </div>

      <div class="buttons">
        <Button class="font-bold button shadow" @click="showBetPopup" :disabled="HasBet">Bet</Button><!--will just invoke function-->
        <Button class="font-bold button shadow" @click="hit" :disabled="currentPlayer != user || AllPlayersHaveBet == false || Standing || !HasBet">Hit</Button><!--will just invoke function-->
        <Button class="font-bold button shadow" @click="stand" :disabled="currentPlayer != user || AllPlayersHaveBet == false || Standing || !HasBet">Stand</Button><!--will just invoke function-->
      </div>
      <div class="footer">
      </div>
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

</style>