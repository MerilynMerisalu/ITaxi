import React from "react";

type Props = {};

const Home = (props: Props) => {
  return (
    <div className="container">
      <main role="main" className="pb-3">
        <div className="text-center">
          <h1 className="display-4">Welcome</h1>
          <p>
            Learn about{" "}
            <a href="https://docs.microsoft.com/aspnet/core">
              building Web apps with ASP.NET Core
            </a>
            .
          </p>
        </div>
      </main>
    </div>
  );
};

export default Home;
