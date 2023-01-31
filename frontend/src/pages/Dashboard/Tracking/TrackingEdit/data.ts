/* This class was created with dummy data only for test purposes */

//This type must be changed according to the database table
export interface Stage {
	isCompleted: boolean
	date: string
	time: string
	stageName: string
}

export const stagesData: Array<Stage> = [
	{
		isCompleted: true,
		date: "05/10/2021",
		time: "12:54",
		stageName: "Waiting List",
	},
	{
		isCompleted: true,
		date: "12/10/2021",
		time: "15:30",
		stageName: "In Progress",
	},
	{
		isCompleted: false,
		date: "12/10/2021",
		time: "15:30",
		stageName: "Shipping",
	},
	{
		isCompleted: false,
		date: "12/10/2021",
		time: "15:30",
		stageName: "Finished",
	},
]

const stagesTypeData: Array<Stage> = [
	{
		isCompleted: true,
		date: "13/10/2021",
		time: "13:20",
		stageName: "Storage 1",
	},
	{
		isCompleted: false,
		date: "12/10/2021",
		time: "15:30",
		stageName: "Storage 2",
	},
	{
		isCompleted: false,
		date: "12/10/2021",
		time: "15:30",
		stageName: "Delivered",
	},
]

const stagesTypeData2: Array<Stage> = [
	{
		isCompleted: true,
		date: "13/10/2022",
		time: "16:20",
		stageName: "Storage 4",
	},
	{
		isCompleted: false,
		date: "12/10/2022",
		time: "17:30",
		stageName: "Storage 5",
	},
	{
		isCompleted: false,
		date: "12/10/2021",
		time: "15:30",
		stageName: "Delivered",
	},
]

const stagesTypeData3: Array<Stage> = [
	{
		isCompleted: true,
		date: "15/10/2022",
		time: "7:20",
		stageName: "Storage 5",
	},
	{
		isCompleted: false,
		date: "20/10/2022",
		time: "10:30",
		stageName: "Storage 6",
	},
	{
		isCompleted: false,
		date: "12/10/2021",
		time: "15:30",
		stageName: "Delivered",
	},
]

export const getRandomStagesTypeData = () => {
	const randomStagesType = [stagesTypeData, stagesTypeData2, stagesTypeData3]
	return randomStagesType[Math.floor(Math.random() * randomStagesType.length)]
}
