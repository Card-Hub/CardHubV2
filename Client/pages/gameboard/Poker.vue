<script setup lang="ts">
import {useUneStore} from "~/stores/uneStore";
import {type ConfigurableDocument, type MaybeElementRef, useFullscreen } from '@vueuse/core';
import {defineComponent, ref, onMounted, type ComputedRef, type Ref, computed} from "vue";
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import StandardnoshadowCard from "~/components/noShadowCard/StandardnoshadowCard.vue";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

const store = useWebSocketStore();
const {user, users, room, connection} = storeToRefs(store);
const {startGame} = store;
// uncomment these out later
// const {  cards, users, room } = storeToRefs(store);
// const { cards } = storeToRefs(store);

// START of HARD CODED DATA
const cardsDisplayed = ref(0);

//const currentTurn = ref<string>(currentPlayer);
const river = ref<StandardCard[]>([
  {Id: 1, Suit: "Spades", Value: "A"},
  {Id: 2, Suit: "Spades", Value: "K"},
  {Id: 3, Suit: "Spades", Value: "Q"},
  {Id: 4, Suit: "Spades", Value: "J"},
  {Id: 5, Suit: "Spades", Value: "10"},
]);

// reverse order of river
river.value.reverse();

// for safe of layout
const players = ref<PokerPlayer[]>([
  {Name: "lyssie", Avatar: "lyssie", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: false},
  {Name: "fairy", Avatar: "fairy", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: false},
  {Name: "juno", Avatar: "juno", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: false},
  {Name: "oli", Avatar: "oli", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: false},
  {Name: "andy", Avatar: "andy", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: false},
  {Name: "liam", Avatar: "liam", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: true},
  {Name: "ruby", Avatar: "ruby", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: false},
  {Name: "alex", Avatar: "alex", Afk: false, CanFold: true, CanCall: true, CanRaise: true, CanCheck: true, Hand: [], BestHand: "", Folded: true},
]);
const currentPlayer = ref<string>("lyssie");
const buttonPlayer = ref<string>("fairy");
const smallBlind = ref<string>("juno");
const bigBlind = ref<string>("oli")

const currentBoardBet = ref<number>(10);
const totalPot = ref<number>(100);

//END of HARD CODED DATA

//fulscreen
const { isFullscreen, enter, exit } = useFullscreen();
const el = ref(null)
const { toggle } = useFullscreen(el)

const getPrimeIcon = (name: string) => {
  return new URL(`../../assets/icons/primeIcons/${name}.svg`, import.meta.url);
}

const cardStyle = (num: number) => {
  let randomX = 360 - (num * 110);
  let randomY = 0;

  return {
    transform: `translate(${randomX}%, ${randomY}%)`,
  };
};

const getSingleCardStyle = (num: number) => {
  let randomX = 200 - (num * 100);
  let randomY = 0;

  return {
    transform: `translate(${randomX}%, ${randomY}%)`,
  };
};

const getPlayerIcon = (player: string) => {
  return new URL(`../../assets/icons/avatars/${player}.png`, import.meta.url);
};

const rivertoDisplay = computed(() => {
  var DC = [] as [StandardCard, number][];
  // grab first 5 cards in discardedCards
  for (let i = 0; i < river.value.length && i < 5; i++) {
    var card: StandardCard = {
      Id: river.value[i].Id,
      Suit: river.value[i].Suit,
      Value: river.value[i].Value
    };

    DC.push([card, 5 - i]);
  }
  return DC;
});

const getPlayerIconStyle = (index: number) => {
  const totalPlayers = players.value.length;
  let xpos = 0;
  let ypos = 0;

  if (totalPlayers === 2) {
    xpos = index === 0 ? 50 : -51;
    ypos = index === 0 ? -350 : 350;
  } else if (totalPlayers === 3) {
    xpos = index === 0 ? 151 : index === 1 ? -276 : 251;
    ypos = index === 0 ? -350 : index === 1 ? 350 : 350;
  } else if (totalPlayers === 4) {
    xpos = index === 0 ? 150 : index === 1 ? -456 : index === 2 ? -56 : 356;
    ypos = index === 0 ? -350 : index === 1 ? 0 : index === 2 ? 350 : 0;
  } else if (totalPlayers === 5) {
    xpos = index === 0 ? 225 : index === 1 ? -475 : index === 2 ? -251 : index === 3 ? 151 : 375;
    ypos = index === 0 ? -300 : index === 1 ? 0 : index === 2 ? 300 : index === 3 ? 300 : 0;
  } else if (totalPlayers === 6) {
    xpos = index === 0 ? 500 : index === 1 ? -70 : index === 2 ? -525 : index === 3 ? -300 : index === 4 ? 60 : 325;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? 0 : index === 3 ? 300 : index === 4 ? 300 : 0;
  } else if (totalPlayers === 7) {
    xpos = index === 0 ? 550 : index === 1 ? -25 : index === 2 ? -425 : index === 3 ? -530 : index === 4 ? -350 : index === 5 ? 0 : 265;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? -150 : index === 3 ? 150 : index === 4 ? 300 : index === 5 ? 300 : 0;
  } else if (totalPlayers === 8) {
    xpos = index === 0 ? 600 : index === 1 ? 30 : index === 2 ? -375 : index === 3 ? -475 : index === 4 ? -300 : index === 5 ? 50 : index === 6 ? 265 : 150;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? -150 : index === 3 ? 150 : index === 4 ? 300 : index === 5 ? 300 : index === 6 ? 150 : -150;
  }
  return {
    transform: `translate(${xpos}%, ${ypos}%)`,
  };
};

const isCurrentPlayer = (player: string) => {
  if (currentPlayer.value != "") {
    if (player.toLowerCase() === currentPlayer.value.toLowerCase()) {
      return {
        border: 'red 3px solid',
        boxShadow: '0 0 10px #D60E26',
      };
    }
  }
};

const isCurrentButtonPlayer = (player: string) => {
  if (buttonPlayer.value != "") {
    if (player.toLowerCase() === buttonPlayer.value.toLowerCase()) {
      return {
        border: 'green 3px solid',
        boxShadow: '0 0 10px #00FF00',
      };
    }
  }
};

const isCurrentBigBlind = (player: string) => {
  if (bigBlind.value != "") {
    if (player.toLowerCase() === bigBlind.value.toLowerCase()) {
      return {
        border: 'blue 3px solid',
        boxShadow: '0 0 10px #0000FF',
      };
    }
  }
};

const isCurrentSmallBlind = (player: string) => {
  if (smallBlind.value != "") {
    if (player.toLowerCase() === smallBlind.value.toLowerCase()) {
      return {
        border: 'yellow 3px solid',
        boxShadow: '0 0 10px #FFFF00',
      };
    }
  }
};

const hasFolded= (player: PokerPlayer) => {
  if (player.Folded) {
    return {
      opacity: '0.5',
    };
  }
};

const getWinnerAvatar = (winner: string) => {
  for (let i = 0; i < players.value.length; i++) {
    if (players.value[i].Name === winner) {
      return players.value[i].Avatar;
    }
  }
  return "lyssie";
};

const particlesLoaded = (container: any) => {
  console.log("Particles loaded", container);
};

const restartGame = () => {
  // startGame();
};

const handleExit = () => {
  connection.value = null;
  users.value = [];
  players.value = [];
  navigateTo("/games");
};
</script>

<template>
  <div class="gameboard-container" ref="el">
    <div class="gameboard">
      <div class="player-icons grid grid-cols-2 content-center">
        <div class="player-icon" v-for="(player, index) in players" :key="index"
             :style="{ ...getPlayerIconStyle(index), ...isCurrentPlayer(player.Name), ...isCurrentButtonPlayer(player.Name), ...isCurrentBigBlind(player.Name), ...isCurrentSmallBlind(player.Name), ...hasFolded(player)}">
          <img :src="getPlayerIcon(player.Avatar)" alt="Player Icon" class="player-icon-img"/>
        </div>
      </div>

      <div class="game-table rounded-tr-full flex flex-col justify-center items-center shadow-xl">
        <h1 class="text-center bet-txt"> CURRENT BET: {{ currentBoardBet }}</h1>
          <div class="deck-view justify-center align-items-center">
            <div class="deck-card flex justify-center items-center rounded-md shadow-md mb-2"
                 v-for="n in 5-cardsDisplayed" :key="n" :style="cardStyle(n)">
            </div>
          </div>

        <div class="">
          <div class="deck-view">
            <StandardnoshadowCard class="singular-card" v-for="(card, index) in river"
                             :key="card.Id"
                             :card="card"
                             :style="getSingleCardStyle(index)"
            />
          </div>
        </div>
        <h1 class="text-center pot-txt"> TOTAL POT: {{ totalPot }}</h1>
      </div>
      
<!--        <div v-if="winner !== ''">-->
<!--          <div class="announcement fade-in flex flex-col justify-center">-->
<!--            <img :src="getPlayerIcon(getWinnerAvatar(winner))" alt="Player Icon" class="winner-icon"/>-->
<!--            <h1> {{ winner }} won! </h1>-->
<!--          </div>-->

<!--          <div class="end-btns">-->
<!--            <Button class="restart-btn end-btns" @click="restartGame()"> Play Again</Button>-->
<!--            <Button class="exit-btn end-btns" @click="handleExit()"> Exit</Button>-->
<!--          </div>-->

<!--          <div class="explosions absolute flex flex-col justify-center items-center">-->
<!--            <div id="app">-->
<!--              <vue-particles id="tsparticles" @particles-loaded="particlesLoaded" url="http://foo.bar/particles.json"/>-->

<!--              <vue-particles-->
<!--                  id="tsparticles"-->
<!--                  @particles-loaded="particlesLoaded"-->
<!--                  :options="{-->
<!--                    background: {-->
<!--                        color: {-->
<!--                            value: 'transparent'-->
<!--                        }-->
<!--                    },-->
<!--                    particles: {-->
<!--                        number: {-->
<!--                            value: 0-->
<!--                        },-->
<!--                        color: {-->
<!--                            value: ['#8338ec', '#ff006e', '#ffbe0b', '#3a86ff']-->
<!--                        },-->
<!--                        animation: {-->
<!--                            enable: true,-->
<!--                            speed: 2,-->
<!--                            startValue: 'max',-->
<!--                            destroy: 'min'-->
<!--                        },-->
<!--                        links: {-->
<!--                            enable: false-->
<!--                        },-->
<!--                        life: {-->
<!--                            count: 1,-->
<!--                            duration: {-->
<!--                              sync: true,-->
<!--                                value: 5-->
<!--                            }-->
<!--                        },-->
<!--                        move: {-->
<!--                            enable: true,-->
<!--                            gravity: {-->
<!--                                enable: true,-->
<!--                                acceleration: 10-->
<!--                            },-->
<!--                          speed: {-->
<!--                              min: 10,-->
<!--                              max: 20-->
<!--                          },-->
<!--                          decay: 0.1,-->
<!--                          direction: 'none',-->
<!--                          straight: false,-->
<!--                          outModes: {-->
<!--                              default: 'destroy',-->
<!--                              top: 'none'-->
<!--                          }-->
<!--                        },-->
<!--                        rotate: {-->
<!--                            value: {min: 0, max: 360},-->
<!--                            move: true,-->
<!--                            direction: 'random',-->
<!--                            animation: {-->
<!--                                enable: true,-->
<!--                                speed: 60-->
<!--                            }-->
<!--                        },-->
<!--                        tilt: {-->
<!--                            enable: true,-->
<!--                            value: {min: 0, max: 360},-->
<!--                            direction: 'random',-->
<!--                            move: true,-->
<!--                            animation: {-->
<!--                                enable: true,-->
<!--                                speed: 60-->
<!--                            }-->
<!--                        },-->
<!--                        roll: {-->
<!--                          darken: {-->
<!--                            enable: true,-->
<!--                            value: 25-->
<!--                          },-->
<!--                          enable: true,-->
<!--                          speed: {-->
<!--                            min: 15,-->
<!--                            max: 25-->
<!--                          }-->
<!--                        },-->
<!--                        wobble: {-->
<!--                            distance: 30,-->
<!--                            enable: true,-->
<!--                            move: true,-->
<!--                            speed: {-->
<!--                                min: -15,-->
<!--                                max: 15}-->
<!--                        },-->
<!--                        opacity: {-->
<!--                            value: {min: 0, max: 2}-->
<!--                        },-->
<!--                        shape: {-->
<!--                            type: ['circle', 'triangle', 'square'],-->
<!--                        },-->
<!--                        size: {-->
<!--                            value: { min: 2, max: 4 }-->
<!--                        }-->
<!--                    },-->
<!--                    emitters: {-->
<!--                      life: {-->
<!--                        count: 0,-->
<!--                        duration: 0.1,-->
<!--                        delay: 0.4-->
<!--                      },-->
<!--                      rate: {-->
<!--                        delay: 0.5,-->
<!--                        quantity: 150-->
<!--                      },-->
<!--                      size: {-->
<!--                        width: 0,-->
<!--                        height: 0-->
<!--                      },-->
<!--                    }-->
<!--                }"-->
<!--              />-->
<!--            </div>-->
<!--          </div>-->
<!--        </div>-->
<!--      </div>-->


    </div>
    <img :src="getPrimeIcon('expand')" class="expand-icon size-14" @click="toggle" />
  </div>

</template>

<style scoped>
.gameboard-container {
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: rgb(243, 19, 19);
  background: radial-gradient(ellipse at center, rgba(243, 19, 19, 0.5) 0%, rgba(152, 14, 17, 0.7) 40%, rgba(63, 8, 14, 0.9) 95%);
}

.gameboard {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}

.player-name {
  font-size: 1.5rem;
  font-weight: bold;
  color: black;
  text-shadow: 2px 2px 4px #000000;
}

.singular-card {
  justify-self: center;
  align-self: center;
  position: absolute;
  animation: flip 0.5s ease-in-out;
  transition: transform 0.5s;
  z-index: 1;
}

@keyframes flip {
  0% {
    transform: perspective(1200px) rotateY(0deg);
    opacity: 0.5;
  }
  40% {
    opacity: 1;
  }
  100% {
    transform: perspective(1200px) rotateY(180deg);
  }
}

.deck-view {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.deck-card {
  position: absolute;
  box-shadow: 0 0 20px rgba(255,255, 255, 0.2);
  transition: transform 0.5s;
  width: 15vw;
  height: 25vw;
  max-height: 250px;
  max-width: 150px;
  margin-right: 100px;
  border: 2px dashed white;
}

.game-table {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 50%;
  max-height: 800px;
  width: 80%;
  max-width: 1000px;
  /*background-color: #f3f4f6;*/
  border-radius: 50% / 100%;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.5);
}

.player-icons {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
  justify-content: center;
  align-items: center;
}

.player-icon {
  width: 100px; /* Adjust the size of the player icon */
  height: 100px; /* Adjust the size of the player icon */
  border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden; /* Ensure the player icon is circular */
  margin-right: 10px; /* Adjust the spacing between player icons */
}

.player-icon-img {
  width: 100%;
  height: 100%;
  object-fit: cover; /* Ensure the player icon fills the circular container */
  background: rgba(255, 255, 255, 0.2); /* Adjust the background color of the player icon and opacity */
}

.winner-icon {
  width: 250px;
  height: 250px;
  border-radius: 50%;
  overflow: hidden;
  margin-right: 10px;
  background: rgba(255, 255, 255, 0.2);
}

.announcement {
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
  background-color: rgb(59, 9, 14);
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
  padding: 20px;
  border-radius: 10%;
  margin-bottom: 20px;
}

.explosions i {
  display: flex;
  justify-content: center;
  align-items: center;
  transform: translate(0%, 900%);
  animation: expand 3s ease forwards;
}

@keyframes expand {
  0% {
    transform: scale(0);
    opacity: 0;
  }
  100% {
    transform: scale(1);
    opacity: 1;
  }
}

.end-btns i {
  animation: expand 3s ease forwards;
}

.restart-btn {
  background-color: #4CAF50; /* Green */
  border: none;
  color: white;
  padding: 15px 32px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
  border-radius: 12px;
  margin-left: 12px;
  margin-right: 20px;
}

.restart-btn:hover {
  background-color: #45a049;
}

.exit-btn {
  background-color: var(--cardhub-red);
  border: none;
  color: white;
  padding: 15px 32px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
  border-radius: 12px;
  margin-left: 20px;
}

.fade-in {
  opacity: 0;
  transform: translateY(-50px); /* Start slightly above */
  animation: fadeIn 2s forwards; /* Animation duration and fill mode */
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-50px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.small-deck-card {
  position: relative;
  width: 75px;
  height: 150px;
  border-radius: 50%;
  overflow: hidden;
  margin-right: 10px;
  background: black;
}

.expand-icon {
  position: absolute;
  bottom: 10px;
  right: 10px;
  cursor: pointer;
}

.bet-txt {
  font-size: 1.5rem;
  font-weight: bold;
  text-shadow: 2px 2px 4px #000000;
  transform: translate(0%, -500%);
}

.pot-txt {
  font-size: 1.5rem;
  font-weight: bold;
  text-shadow: 2px 2px 4px #000000;
  transform: translate(0%, 500%);
}
</style>