import { useState } from 'react';
import ListTracking from '../ListTracking';

import { FaFileInvoice, FaRegCalendarAlt } from 'react-icons/fa';
import { MdGroups } from 'react-icons/md';

import HomeImage from '../../../assets/home_image.png';

export default function Home() {
  const [isVisible, setIsVisible] = useState(false);

  return (
    <div className="mt-8">
      <div className="w-3/4 mx-auto flex rounded-2xl bg-white text-black mb-14 shadow-lg border border-gray-100">
        <div className="w-full lg:w-3/4 mx-11 mt-7">
          <h1 className="font-bold text-xl text-gray-600 mb-6">
            Track your oder!
          </h1>
          <div className="relative flex mb-6 lg:w-11/12">
            <div className="relative flex items-stretch flex-grow focus-within:z-10">
              <input
                type="text"
                className="input !rounded-none !rounded-l-md"
                placeholder="Insert the order code. For example: OD129387"
              />
            </div>
            <button
              type="button"
              onClick={() => {
                setIsVisible((prev) => !prev);
              }}
              className="-ml-px relative inline-flex items-center space-x-2 px-10 py-2 border border-gray-300 text-sm font-medium rounded-r-md text-gray-700 bg-gray-50 hover:bg-gray-100 focus:outline-none focus:ring-offset-0"
            >
              <span>Search</span>
            </button>
          </div>
          <div className="w-full 2xl:w-10/12 mb-4 text-sm text-gray-500">
            <p>
              Track your lot with ease. Enter your unique code and view the
              progress of your lots in real time. Our blockchain-based system
              ensures accuracy and security.
            </p>
          </div>
        </div>
        <div className="hidden lg:flex lg:w-1/4">
          <img className="w-full  rounded-r-2xl object-cover" src={HomeImage} />
        </div>
      </div>

      <div className="w-5/6 md:w-3/4 mx-auto mb-14">
        <div className="flex flex-wrap justify-center text-center mx-auto w-2/3">
          <h1 className="text-3xl font-bold mb-8">
            Welcome to our website, where you can experience the future of
            production traceability
          </h1>
          <span className="text-md font-medium text-justify">
            Our state-of-the-art system utilizes blockchain technology to record
            every step of the production process, ensuring complete transparency
            and accountability.<br></br>
            By using our platform, you can easily trace the journey of your
            product, from raw materials to final delivery, with complete
            accuracy and security.<br></br>
            With our platform, you can have peace of mind knowing that your
            products are ethically sourced and produced, and that your customers
            have access to the information they need to make informed purchasing
            decisions.<br></br>
            In addition, our platform is user-friendly, easy to navigate and
            accessible from any device. So you can access information about your
            products from anywhere, at any time.<br></br>
            We invite you to explore our website and discover how our platform
            can help you achieve a new level of transparency and trust in your
            production process.<br></br>
            Join us now and be a part of the future of traceability.
          </span>
        </div>
      </div>

      <div className="flex flex-wrap w-3/4 mx-auto">
        <div className="lg:pt-12 pt-6 w-full md:w-4/12 md:pr-6 text-center">
          <div className="relative flex flex-col min-w-0 break-words bg-white w-full mb-8 shadow-lg rounded-2xl border border-gray-100">
            <div className="px-4 py-5 flex-auto">
              <div className="text-white items-center text-center flex mx-auto  justify-center w-16 h-16 -mt-12 mb-10 shadow-lg rounded-full bg-indigo-500">
                <FaFileInvoice size={30} />
              </div>
              <h6 className="text-xl font-semibold">
                Boost Your Sales and Brand Reputation with Blockchain
                Traceability
              </h6>
              <p className="mt-2 mb-4 mx-8 text-blueGray-500 text-justify">
                Introducing our revolutionary blockchain-based production
                traceability system. Our platform allows you to provide complete
                transparency and accountability throughout the entire production
                process, giving your customers the assurance that your products
                are ethically sourced and produced. With this competitive edge,
                you'll be able to differentiate your products from your
                competitors, building trust and loyalty with your customers,
                leading to increased sales and a stronger brand reputation. Get
                ahead of the competition and try our platform today!
              </p>
            </div>
          </div>
        </div>
        <div className="lg:pt-12 pt-6 w-full md:w-4/12 md:px-3 text-center">
          <div className="relative flex flex-col min-w-0 break-words bg-white w-full mb-8 shadow-lg rounded-2xl border border-gray-100">
            <div className="px-4 py-5 flex-auto">
              <div className="text-white items-center text-center flex mx-auto  justify-center w-16 h-16 -mt-12 mb-10 shadow-lg rounded-full bg-orange-400">
                <FaRegCalendarAlt size={30} />
              </div>
              <h6 className="text-xl font-semibold">
                Protect Your Supply Chain and Brand with Blockchain Traceability
              </h6>
              <p className="mt-2 mb-4 mx-8 text-blueGray-500 text-justify">
                Protect your brand and supply chain with our blockchain-based
                production traceability system. Our platform records every step
                of the production process on the blockchain, giving you complete
                visibility and control over your supply chain. This helps you
                ensure that your products are ethically sourced and produced,
                allowing you to comply with industry regulations and avoid
                costly recalls or reputational damage. Take control of your
                supply chain and safeguard your brand's reputation with our
                platform today!
              </p>
            </div>
          </div>
        </div>
        <div className="lg:pt-12 pt-6 w-full md:w-4/12 md:pl-6  text-center">
          <div className="relative flex flex-col min-w-0 break-words bg-white w-full mb-8 shadow-lg rounded-2xl border border-gray-100">
            <div className="px-4 py-5 flex-auto">
              <div className="text-white items-center text-center flex mx-auto  justify-center w-16 h-16 -mt-12 mb-10 shadow-lg rounded-full bg-cyan-500">
                <MdGroups size={30} />
              </div>
              <h6 className="text-xl font-semibold">
                Streamline Your Production and Increase Efficiency with
                Blockchain Traceability
              </h6>
              <p className="mt-2 mb-4 mx-8 text-blueGray-500 text-justify">
                Streamline your production process and increase efficiency with
                our blockchain-based traceability system. With all production
                data recorded on the blockchain, you can easily access and share
                information with suppliers, logistics providers, and other
                stakeholders. This leads to faster time to market, reduced
                costs, and improved efficiency. Our platform is user-friendly
                and accessible from any device, allowing you to access
                information about your products from anywhere, at any time. Get
                ahead of the competition and try our platform today!
              </p>
            </div>
          </div>
        </div>
      </div>
      <ListTracking open={isVisible} handleVisibility={setIsVisible} />
    </div>
  );
}
