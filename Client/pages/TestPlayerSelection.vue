<script setup lang="ts">
import { ref } from 'vue';
import AvatarSelection from "~/components/AvatarSelection.vue";

const isPlayer = ref<boolean>(true);

</script>

<template>
  <div id="app">
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

