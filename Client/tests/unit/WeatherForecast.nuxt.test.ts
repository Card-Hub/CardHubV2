import { expect, test } from 'vitest'

function sum(a: number, b: number): number {
    return a + b
}

test('check vitest w/ no other dependencies', () => {
    expect(sum(1, 2)).toBe(3)
})



import { mount } from '@vue/test-utils'

// The component to test
const MessageComponent = {
    template: '<p>{{ msg }}</p>',
    props: ['msg']
}

test('check vitest w/ Vue Test Utils', () => {
    const wrapper = mount(MessageComponent, {
        props: {
            msg: 'Hello world'
        }
    })

    // Assert the rendered text of the component
    expect(wrapper.text()).toContain('Hello world')
})


import WeatherForecast from "~/pages/WeatherForecast.vue";

test('renders a city', async () => {
    const wrapper = mount(WeatherForecast, {
        props: {
            cities: ["Reno"]
        }
    })

    const city = wrapper.get('[data-test="city"]')
    console.log(city.text())
    expect(city.text()).toBe("Reno")
})

// import TodoApp from "~/pages/TodoApp.vue";
//
// test('renders a todo', () => {
//     const wrapper = mount(TodoApp)
//
//     const todo = wrapper.get('[data-test="todo"]')
//
//     expect(todo.text()).toBe('Learn Vue.js 3')
// })