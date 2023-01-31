import { StagesModelStage } from "../../models/api/StagesModelStage"
import { MyAPI } from "../MyAPI"

export const getStagesModelStageByStageModelId = async (stageModelId: string = "") => {
	const response = await MyAPI().get(
		`/StagesModelStage/getStagesModelStage?id=${stageModelId}&page=1&perPage=1000`
	)
	return response.data
}

export const addStagesModelStage = async (stagesModelStage: StagesModelStage) => {
	const response = await MyAPI().post("/StagesModelStage", stagesModelStage)
	console.log(
		"Added stageModelStage stagesId -> " +
			stagesModelStage.stagesId +
			" stagesModelId -> " +
			stagesModelStage.stagesModelId
	)
	return response.data
}

export const updateStagesModelStage = async (stagesModelStageList: Array<StagesModelStage>) => {
	const response = await MyAPI().put("/StagesModelStage", stagesModelStageList)

	return response.data
}

export const removeStagesModelStage = async (stageModelId: string) => {
	const response = await MyAPI().delete(
		`/StagesModelStage/removeAllStagesModelStageByStageModelId?StageModelId=${stageModelId}`
	)

	return response.data
}
