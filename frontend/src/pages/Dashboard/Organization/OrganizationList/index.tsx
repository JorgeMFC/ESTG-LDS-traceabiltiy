import { Button } from "flowbite-react/lib/esm/components/Button"

import { ComponentTypesSection } from "./components/ComponentTypesSection"
import { StagesSection } from "./components/StagesSection"
import "./style.scss"

export default function OrganizationList() {
	return (
		<div className="relative mx-auto bg-white rounded-2xl min-h-full sm:w-4/5">
			<div className="p-8">
				<div className="flex items-center absolute top-2 right-2">
					<img
						className="w-12 h-12 sm:w-16 sm:h-16 object-cover rounded-lg"
						src="https://img1.wsimg.com/isteam/ip/eed91d6b-838b-4d04-b670-9f886e21dc70/logomini2.png"
						alt=""
					/>
					<h1 className="font-bold text-lg">Coincal LDA</h1>
				</div>

				<h1 className="text-2xl font-bold mt-6 sm:mt-0 text-gray-600">Organization Details</h1>

				<div className="grid grid-cols-1 sm:grid-cols-2 mt-10 gap-2">
					<div className="flex justify-center">
						<img
							className="w-64 h-44 lg:w-96 lg:h-64 rounded-lg"
							src="https://images.adsttc.com/media/images/584e/4b0f/e58e/ce89/a700/018a/large_jpg/130421001.jpg?1481526017"
							alt=""
						/>
					</div>
					<div className="flex flex-col gap-4 px-10">
						<div>
							<label className="block text-sm font-medium text-gray-700">Name *</label>
							<div className="relative mt-1 rounded-md shadow-sm">
								<input
									type="text"
									className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
									placeholder="Organization Name"
								/>
							</div>
						</div>

						<div>
							<label className="block text-sm font-medium text-gray-700">Email *</label>
							<div className="relative mt-1 rounded-md shadow-sm">
								<input
									type="text"
									className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
									placeholder="Organization Email"
								/>
							</div>
						</div>

						<div>
							<label className="block text-sm font-medium text-gray-700">Address *</label>
							<div className="relative mt-1 rounded-md shadow-sm">
								<input
									type="text"
									className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
									placeholder="Organization Address"
								/>
							</div>
						</div>

						<div className="flex justify-end gap-4">
							<Button color="dark" size="md">
								Select Photo
							</Button>
							<Button color="dark" size="md">
								Select Logo
							</Button>
							<Button color="dark" size="md" className="!h-full">
								Save
							</Button>
						</div>
					</div>
				</div>

				<StagesSection />
				<ComponentTypesSection />
			</div>
		</div>
	)
}
