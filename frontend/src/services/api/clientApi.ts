import { Client, ClientAddressList } from "../../models/api/Client"
import { MyAPI } from "../MyAPI"

export const getClients = async (page: number = 1, perPage: number = 10, search: string = "") => {
	const response = await MyAPI().get(
		`Client/getClients?page=${page}&perPage=${perPage}&search=${search}`
	)
	return response.data
}

const addClient = async (props: Client) => {
	return await MyAPI().post("/Client", props)
}

export const addClientWithAddresses = async (props: ClientAddressList) => {
	const result = await MyAPI().post("/Client/addClientList", props)
	return result.data
}

export const getClientAddressesByClientId = async (clientId: string) => {
	const response = await MyAPI().get(`Client/getClientAddressByClientId?clientId=${clientId}`)
	return response.data
}

export const removeClient = async (clientId: string) => {
	const result = await MyAPI().delete(`/Client?id=${clientId}`)
	return result.data
}
