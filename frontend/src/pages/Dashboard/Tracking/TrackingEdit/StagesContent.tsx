import { getRandomStagesTypeData, Stage } from "./data"

interface StagesContentProps {
	stages: Array<Stage>
	setStagesType?: React.Dispatch<React.SetStateAction<any>>
}

export const StagesContent = (props: StagesContentProps) => {
	return (
		<div className="flex justify-center space-x-4 w-full">
			<ul className="flex flex-col justify-between">
				{props.stages.map((stage, index) =>
					stage.isCompleted ? (
						<li className="flex flex-col h-20" key={index}>
							<span className="mr-4 text-sm font-semibold text-gray-800">{stage.date}</span>
							<span className="mr-4 text-xs font-normal text-gray-400">{stage.time}</span>
						</li>
					) : (
						<li className="flex h-20" key={index}>
							<span className="mr-4 text-sm font-normal text-gray-400">{stage.stageName}</span>
						</li>
					)
				)}
			</ul>

			{/* The setStagesType must be changed with the substages of the selected stage when clicking
            on the button (This data must come from an api request) */}
			<ul className="relative h-4/5 border-l border-gray-200 border-dashed">
				{props.stages.map((stage, index) => (
					<li className="pb-20" key={index}>
						{props.setStagesType ? (
							<div
								className="absolute w-6 h-6 bg-blue-600 rounded-full -left-3 border border-white 
						cursor-pointer hover:bg-blue-700"
								onClick={() =>
									props.setStagesType && props.setStagesType(getRandomStagesTypeData())
								}
							></div>
						) : (
							<div className="absolute w-6 h-6 bg-blue-600 rounded-full -left-3 border border-white"></div>
						)}
					</li>
				))}
			</ul>

			<ul className="flex flex-col justify-between">
				{props.stages.map((stage, index) => (
					<li className="flex h-20" key={index}>
						<span className="ml-4 text-sm font-semibold text-gray-400">{stage.stageName}</span>
					</li>
				))}
			</ul>
		</div>
	)
}
