import { test, describe, mount, expect } from 'vitest';
import PlayerView from '../../pages/PlayerView.vue';
import StandardCardDisplay from "../../components/Card/StandardCardDisplay.vue";
import PlayerHand from "../../components/PlayerHand.vue";
import UNOCardDisplay from "../../components/Card/UNOCardDisplay.vue";

describe('PlayerView', () => {
  test('renders standard card correctly', () => {
    const standardCard = { id: 1, suit: 'hearts', value: '10' };

    const wrapper = mount(PlayerHand, {
      props: {
        playerHand: [standardCard]
      }
    });

    expect(wrapper.exists()).toBe(true, 'PlayerHand component should exist');
    expect(wrapper.findComponent(StandardCardDisplay).exists()).toBe(true, 'StandardCardDisplay component should be rendered for standard card');
  });

  test('renders UNO card correctly', () => {
    const unoCard = { id: 1, color: '#d12c15', value: 'Reverse' };

    const wrapper = mount(PlayerView, {
      props: {
        playerHand: [unoCard]
      }
    });

    expect(wrapper.exists()).toBe(true, 'PlayerView component should exist');
    expect(wrapper.findComponent(UNOCardDisplay).exists()).toBe(true, 'UNOCardDisplay component should be rendered for UNO card');
  });
});