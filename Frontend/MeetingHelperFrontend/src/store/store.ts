import {configureStore} from "@reduxjs/toolkit"
import aiResponseSlice from "./aiResponseSlice"

export const store = configureStore({
    reducer: {
        aiResponse : aiResponseSlice
    }
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch;