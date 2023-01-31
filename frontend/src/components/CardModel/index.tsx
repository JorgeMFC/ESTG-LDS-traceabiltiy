import { AiOutlineBgColors } from "react-icons/ai"
import { TrackingIcons } from "../../models/TrackingIcons"

import { BiTargetLock } from "react-icons/bi"
import { FiUser } from "react-icons/fi"
import { HiOutlineDocumentReport } from "react-icons/hi"
import { IoMdResize } from "react-icons/io"

export interface CardModelProps {
	imageSrc: string
	modelName: string
	lotId: string
	amount: number
	stageModelName: string
	clientName: string
	modelColor: string
	modelSize: string
	icon: TrackingIcons
	action: () => void
}

export default function CardModel(props: CardModelProps) {
	return (
		<div
			onClick={() => props.action()}
			className="cursor-pointer mb-8 flex flex-row items-center bg-white rounded-3xl border shadow-md md:max-w-md hover:bg-white-100 border-white-700 bg-white-800 hover:bg-white-700 "
		>
			<div className="flex flex-col w-full justify-between p-4 leading-normal">
				<div
					className={`text-white text-2xl items-center text-center flex justify-center w-12 h-12 -mt-10 mb-5 shadow-lg rounded-full ${props.icon.color}`}
				>
					<props.icon.icon />
				</div>
				<span className="text-xs text-gray-400 font-normal">{props.lotId}</span>

				<h5 className="mb-2 text-xl font-bold line-clamp-1 text-white-900">{props.modelName}</h5>
				<div className="flex pb-3">
					<BiTargetLock className="flex-none w-6 h-full" />
					<span className="ml-2 truncate">{props.stageModelName}</span>
				</div>

				<div className="flex pb-2">
					<HiOutlineDocumentReport className="flex-none w-6 h-full" />
					<span className="ml-2 truncate">{props.amount}</span>
				</div>
				<hr className="h-px bg-gray-200 border-0" />
				<div className="flex py-2">
					<FiUser className="flex-none w-6 h-full" />
					<span className="ml-2 truncate">{props.clientName}</span>
				</div>
				<div className="flex py-2">
					<AiOutlineBgColors className="flex-none w-6 h-full" />
					<span className="ml-2 truncate">{props.modelColor}</span>
				</div>
				<div className="flex py-2">
					<IoMdResize className="flex-none w-6 h-full" />
					<span className="ml-2 truncate">{props.modelSize}</span>
				</div>
			</div>
			<img className="object-cover h-full w-32 xxs:w-40 rounded-r-2xl" src={props.imageSrc} />
		</div>
	)
}
