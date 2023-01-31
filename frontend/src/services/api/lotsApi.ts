import { Lot } from "../../models/api/Lot"
import { MyAPI } from "../MyAPI"

export const getLots = async (search: string = "") => {
	const response = await MyAPI().get(`Lot?page=1&perPage=20&search=${search}`)
	return response.data
}

export const addLot = async (props: Lot) => {
	const response = await MyAPI().post(`Lot`, props)
	return response.data
}

export const createNFT = async (props: Lot) => {
	const response = await MyAPI().post(`Lot/CreateNFTLot`, props)
	return response.data
}
