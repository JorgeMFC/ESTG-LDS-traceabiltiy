import { Stage } from "../../models/api/Stage"
import { MyAPI } from "../MyAPI"

export const getStages = async () => {
	const response = await MyAPI().get("/Stages?page=1&perPage=1000")
	return response.data
}

export const addStage = async (props: { stage: Stage }) => {
	return await MyAPI().post("/Stages", props.stage)
}

export const deleteStage = async (props: { id: string }) => {
	return await MyAPI().delete(`/Stages?id=${props.id}`)
}
