<script setup lang="ts">
import {useUneStore} from "~/stores/uneStore";
import {type ConfigurableDocument, type MaybeElementRef, useFullscreen } from '@vueuse/core';
import {defineComponent, ref, onMounted, type ComputedRef, type Ref, computed} from "vue";
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";

const store = useWebSocketStore();
const {user, users, room, connection} = storeToRefs(store);
const {startGame} = store;
const uneStore = useUneStore();
// uncomment these out later
// const {  cards, users, room } = storeToRefs(store);
// const { cards } = storeToRefs(store);

const newCards = ref<number[]>([]);
//const currentColor = ref<string>("red");
const {currentColor, players, winner, currentPlayer, discardPile, direction} = storeToRefs(uneStore);

// const currentTurn = ref<string>("fairy");
//const currentTurn = ref<string>(currentPlayer);
const cards = ref<UNOCard[]>(discardPile);

//fulscreen
const { isFullscreen, enter, exit } = useFullscreen();
const el = ref(null)
const { toggle } = useFullscreen(el)

const getCardStyle = (index: number) => {
  const randomX = Math.floor(Math.random() * 50) - 5; // Random offset for X-axis
  const randomY = Math.floor(Math.random() * 50) - 5; // Random offset for Y-axis
  const randomRotation = Math.floor(Math.random() * 20) - 10; // Random rotation

  return {
    transform: `translate(${randomX}px, ${randomY}px) rotate(${randomRotation}deg)`,
    zIndex: index,
  };
};

const getPrimeIcon = (name: string) => {
  return new URL(`../../assets/icons/primeIcons/${name}.svg`, import.meta.url);
}

const cardStyle = (num: number) => {
  let randomX = num * 5;
  let randomY = num * 3;
  let randomRotation = 0;

  return {
    transform: `translate(${randomX}px, ${randomY}px) rotate(${randomRotation}deg)`,
    zIndex: num,
  };
};

const getPlayerIcon = (player: string) => {
  return new URL(`../../assets/icons/avatars/${player}.png`, import.meta.url);
};

const discardedCardsToDisplay = computed(() => {
  var DC = [] as [UNOCard, number][];
  // grab first 5 cards in discardedCards
  for (let i = 0; i < discardPile.value.length && i < 5; i++) {
    var card: UNOCard = {
      Id: discardPile.value[i].Id,
      Color: discardPile.value[i].Color,
      Value: discardPile.value[i].Value
    };
    if (discardPile.value[i].Value === "Wild" || discardPile.value[i].Value === "Wild Draw Four") {
      card.Color = currentColor.value;
    }
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

const getWinnerAvatar = (winner: string) => {
  for (let i = 0; i < players.value.length; i++) {
    if (players.value[i].Name === winner) {
      return players.value[i].Avatar;
    }
  }
  return "lyssie";
};

const getUNE = () => {
  return new URL(`../../assets/icons/unoDeck/UNE.svg`, import.meta.url);
};

const getNumofCards = (player: unePlayer) => {
  let hand: UNOCard[] = player.Hand;
  return hand.length;
};

const getArrow = () => {
  return new URL(`../../assets/icons/directionArrows/${currentColor.value.toLowerCase()}Arrow.svg`, import.meta.url);
};

const getleftArrow = () => {
  let angle = 180;
  let scale = 1;
  if (direction.value.toLowerCase() === "clockwise") {
    angle = 0;
    scale = -1;
  }

  return {
    transform: `rotate(${angle}deg) scaleX(${scale})`,
  };
};

const getRightArrow = () => {
  let angle = 0;
  let scale = 1;
  if (direction.value.toLowerCase() === "clockwise") {
    angle = 180;
    scale = -1;
  }

  return {
    transform: `rotate(${angle}deg) scaleX(${scale})`,
  };
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
  <!--  <p> {{ currentPlayer }}</p>-->
  <!--  <p> {{ discardPile }}</p>-->
  <!--  <p> {{ discardedCardsToDisplay }}</p>-->
<!--  <UseFullscreen v-slot="{ toggle }">-->
  <div class="gameboard-container" ref="el">
    <div class="gameboard">
      <div class="player-icons grid grid-cols-2 content-center">
        <div class="player-icon" v-for="(player, index) in players" :key="index"
             :style="{ ...getPlayerIconStyle(index), ...isCurrentPlayer(player.Name) }">
          <img :src="getPlayerIcon(player.Avatar)" alt="Player Icon" class="player-icon-img"/>
          
<!--          <p> {{ players[index].Name.length }}</p>-->
        </div>
      </div>
<!--      <div class="" v-for="player in players" :key="player">-->
<!--        <h1 class="player-name">{{ player.Name }} </h1>-->
<!--      </div>-->
<!--      <div class="num-cards-container">-->
<!--        <div class="uno-small-card">-->
<!--          <div class="small-deck-card flex justify-center items-center bg-zinc-800 rounded-md shadow-md mb-2">-->
<!--            <img :src="getUNE()" alt="game icon" class="une-logo"/>-->
<!--          </div>-->
<!--          <p> xHello </p>-->
<!--        </div>-->
<!--      </div>-->

      <div class="game-table rounded-tr-full shadow-lg" v-bind:class="currentColor">
        <div v-if="winner === ''">
          <div class="h-auto w-[900px] grid grid-cols-4 gap-4 place-items-center">
            
            <img :src="getArrow()" alt="left arrow" class="arrow"
                 :style="getleftArrow()"/>
            
            <div class="">
              <div class="deck-view">
                <!--            <div class="flex justify-center items-center w-52 h-80 bg-zinc-800 rounded-md shadow-md mb-2">-->
                <div class="deck-card flex justify-center items-center bg-zinc-800 rounded-md shadow-md mb-2"
                     v-for="n in 5" :key="n" :style="cardStyle(n)">
                  <img :src="getUNE()" alt="game icon" class="une-logo"/>
                </div>
              </div>
            </div>

            <div class="">
              <div class="deck-view">
                <UNEnoshadowCard class="singular-card" v-for="[card, index] in discardedCardsToDisplay"
                                 :key="card.Id"
                                 :card="card"
                                 :style="{ ...getCardStyle(index) }"
                />
              </div>
            </div>

            <img :src="getArrow()" alt="right arrow" class="arrow"
                 :style="getRightArrow()"/>
            
          </div>
        </div>
        <div v-if="winner !== ''">
          <div class="announcement fade-in flex flex-col justify-center">
            <img :src="getPlayerIcon(getWinnerAvatar(winner))" alt="Player Icon" class="winner-icon"/>
            <h1> {{ winner }} won! </h1>
          </div>

          <div class="end-btns">
            <Button class="restart-btn end-btns" @click="restartGame()"> Play Again</Button>
            <Button class="exit-btn end-btns" @click="handleExit()"> Exit</Button>
          </div>

          <div class="explosions absolute flex flex-col justify-center items-center">
            <div id="app">
              <vue-particles id="tsparticles" @particles-loaded="particlesLoaded" url="http://foo.bar/particles.json"/>

              <vue-particles
                  id="tsparticles"
                  @particles-loaded="particlesLoaded"
                  :options="{
                    background: {
                        color: {
                            value: 'transparent'
                        }
                    },
                    particles: {
                        number: {
                            value: 0
                        },
                        color: {
                            value: ['#8338ec', '#ff006e', '#ffbe0b', '#3a86ff']
                        },
                        animation: {
                            enable: true,
                            speed: 2,
                            startValue: 'max',
                            destroy: 'min'
                        },
                        links: {
                            enable: false
                        },
                        life: {
                            count: 1,
                            duration: {
                              sync: true,
                                value: 5
                            }
                        },
                        move: {
                            enable: true,
                            gravity: {
                                enable: true,
                                acceleration: 10
                            },
                          speed: {
                              min: 10,
                              max: 20
                          },
                          decay: 0.1,
                          direction: 'none',
                          straight: false,
                          outModes: {
                              default: 'destroy',
                              top: 'none'
                          }
                        },
                        rotate: {
                            value: {min: 0, max: 360},
                            move: true,
                            direction: 'random',
                            animation: {
                                enable: true,
                                speed: 60
                            }
                        },
                        tilt: {
                            enable: true,
                            value: {min: 0, max: 360},
                            direction: 'random',
                            move: true,
                            animation: {
                                enable: true,
                                speed: 60
                            }
                        },
                        roll: {
                          darken: {
                            enable: true,
                            value: 25
                          },
                          enable: true,
                          speed: {
                            min: 15,
                            max: 25
                          }
                        },
                        wobble: {
                            distance: 30,
                            enable: true,
                            move: true,
                            speed: {
                                min: -15,
                                max: 15}
                        },
                        opacity: {
                            value: {min: 0, max: 2}
                        },
                        shape: {
                            type: ['circle', 'triangle', 'square'],
                        },
                        size: {
                            value: { min: 2, max: 4 }
                        }
                    },
                    emitters: {
                      life: {
                        count: 0,
                        duration: 0.1,
                        delay: 0.4
                      },
                      rate: {
                        delay: 0.5,
                        quantity: 150
                      },
                      size: {
                        width: 0,
                        height: 0
                      },
                    }
                }"
              />
            </div>
          </div>
        </div>
      </div>


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

  transition: transform 0.5s;
}

.deck-view {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}

.deck-card {
  position: absolute;
  border: grey 2px solid;
  transition: transform 0.5s;
  width: 20vw;
  height: 30vw;
  max-height: 300px;
  max-width: 200px;
  margin-right: 100px;
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
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
}

.une-logo {
  width: 100%;
  height: 100%;
  max-height: 300px;
  max-width: 200px;
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

.arrow {
  height: 30%;
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
</style>