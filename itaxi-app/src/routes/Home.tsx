import React from "react";
import { JwtContext } from "./Root";

type Props = {};

const Home = (props: Props) => {
  return (
    <div className="container">
      <main role="main" className="pb-3">
        <div className="text-center">
          <h1 className="display-4">ITaxi</h1>
          <p>
            Welcome to ITAXI {JwtContext.displayName}
            .
          </p>
        </div>
      </main>
    </div>
  );
};

export default Home;
