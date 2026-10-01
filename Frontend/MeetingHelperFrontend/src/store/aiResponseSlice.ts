import { createSlice, type PayloadAction } from "@reduxjs/toolkit"

export interface AiResponse{
    response: string
}

const initialState : AiResponse = {
    response: ""
}

const aiResponseSlice = createSlice({
    name: "response",
    initialState,
    reducers: {
        setResponse: (state, action: PayloadAction<string>) => {
            state.response = action.payload;
        },
        clearResponse: (state) => {
            state.response = "";
        }
    }
})

export const {setResponse, clearResponse} = aiResponseSlice.actions;
export default aiResponseSlice.reducer;