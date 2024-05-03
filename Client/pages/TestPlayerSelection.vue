<script setup lang="ts">
import { ref } from 'vue';
import AvatarSelection from "~/components/AvatarSelection.vue";

const isPlayer = ref<boolean>(true);

// import { useSound } from '@vueuse/sound';
import sillyFun from '../assets/audio/music/sillyFun.mp3';
import sway from '../assets/audio/music/sway.mp4';
import drawCard from '../assets/audio/soundEffects/drawCard.mp4';
import newMessage from '../assets/audio/soundEffects/newMessage.mp4';
import newPlayer from '../assets/audio/soundEffects/newPlayer.mp4';
import playCard from '../assets/audio/soundEffects/playCard.mp4';
import sendMessage from '../assets/audio/soundEffects/sendMessage.mp4';
import Dialog from 'primevue/dialog';
const settingsVisible = ref(false);
const bgVolume = ref(0.5);
const seVolume = ref(0.5);

//sound effect
const seDC = new Audio(drawCard);
const seNM = new Audio(newMessage);
const seNP = new Audio(newPlayer);
const sePC = new Audio(playCard);
const seSM = new Audio(sendMessage);

// const backgroundMusic = new Audio(sillyFun);
const backgroundMusic = new Audio(sway);
// const backgroundMusic = new Audio(sillyFun);

// const playSillyFun = () => {
//   // const { play } = u;
//   backgroundMusic.loop = true;
//   backgroundMusic.play();
// };
// playSillyFun();

// SOUND EFFECTS
const playDrawCard = () => {
  seDC.volume = seVolume.value;
  seDC.play();
};
const playNewMessage = () => {
  seNM.volume = seVolume.value;
  seNM.play();
};
const playNewPlayer = () => {
  seNP.volume = seVolume.value;
  seNP.play();
};
const playPlayCard = () => {
  sePC.volume = seVolume.value;
  sePC.play();
};
const playSendMessage = () => {
  seSM.volume = seVolume.value;
  seSM.play();
};

const updateSEVolume = () => {
  seDC.volume = seVolume.value;
  seNM.volume = seVolume.value;
  seNP.volume = seVolume.value;
  sePC.volume = seVolume.value;
  seSM.volume = seVolume.value;
};

const seVolumeDown = () => {
  seVolume.value = 0;
  updateSEVolume();
};

const seVolumeUp = () => {
  seVolume.value = 1;
  updateSEVolume();
};
// SOUND EFFECTS END

const updateBGVolume = () => {
  backgroundMusic.volume = bgVolume.value;
};

const bgVolumeDown = () => {
  bgVolume.value = 0;
  updateBGVolume();
};

const bgVolumeUp = () => {
  bgVolume.value = 1;
  updateBGVolume();
};
</script>

<template>
<!--  <audio ref="audioPlayer" src="../assets/audio/music/sway.mp4" loop></audio>-->
  <div id="app">
    <Button @click="playSillyFun">Toggle Player</Button>

<!--    <i class="pi pi-fw pi-cog" style="font-size: 2rem" @click="settingsVisible = true"></i>-->
<!--    <Dialog v-model="settingsVisible" class="w-auto h-auto" header="Settings" :visible="settingsVisible" @update:visible="settingsVisible = $event">       -->
<!--        <h1 class="text-center">Background Music</h1> <br>-->
<!--        <div class="flex flex-row justify-content-center align-middle">-->
<!--          <i class="pi pi-fw pi-volume-down" style="font-size: 2rem" @click="bgVolumeDown"></i>-->
<!--          <div class="content-around">-->
<!--            <Slider v-model="bgVolume" :min="0" :max="1" :step="0.1" class="w-96" @change="updateBGVolume"/>-->
<!--          </div>-->
<!--          <i class="pi pi-fw pi-volume-up" style="font-size: 2rem" @click="bgVolumeUp"></i>-->
<!--        </div>-->
<!--        -->
<!--        <br>-->
<!--        <h1 class="text-center">Sound Effects</h1> <br>-->
<!--        <div class="flex flex-row justify-content-center align-middle">-->
<!--          <i class="pi pi-fw pi-volume-down" style="font-size: 2rem" @click="seVolumeDown"></i>-->
<!--          <div class="content-around">-->
<!--            <Slider v-model="seVolume" :min="0" :max="1" :step="0.1" class="w-96" @change="updateSEVolume"/>-->
<!--          </div>-->
<!--          <i class="pi pi-fw pi-volume-up" style="font-size: 2rem" @click="seVolumeUp"></i>-->
<!--        </div><br>-->
<!--    </Dialog>-->
    <!--                  <tsparticles id="tsparticles" @particlesLoaded="particlesLoaded" :options="options" />                  -->
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
</template>

<style scoped>

.card {
  background: rgba( 255, 255, 255, 0.1 );
  box-shadow: 0 8px 32px 0 rgba( 74, 1, 29, 0.15);
  backdrop-filter: blur( 20px );
  -webkit-backdrop-filter: blur( 20px );
  border-radius: 28px;
  border: 1px solid rgba( 255, 255, 255, 0.10 );
}

.suit {
  opacity: 20%;
}
</style>

