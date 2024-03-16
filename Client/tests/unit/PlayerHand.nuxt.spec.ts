import { expect, it } from "vitest";
import { mount } from "@vue/test-utils";
import PlayerHand from '#components';

it("can mount some component", async () => {
    const unoCards: UNOCard[] =
        [{
            id: 1,
            value: "1",
            color: "red"
        }, {
            id: 1,
            value: "1",
            color: "red"
        }, {
            id: 1,
            value: "1",
            color: "red"
        }];

    const wrapper = mount(PlayerHand, {
        props: {
            playerHand: unoCards
        },
        shallow: false
    });
    expect(wrapper.attributes("class")).toBe("player-hand");
});

// import { expect, test } from 'vitest'
//
// function sum(a: number, b: number): number {
//     return a + b
// }
//
// test('adds 1 + 2 to equal 3', () => {
//     expect(sum(1, 2)).toBe(3)
// })

// https://test-utils.vuejs.org/api/#components
// https://vuejs.org/guide/scaling-up/testing