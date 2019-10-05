using System;
using System.Linq;
using Xunit;
using BomClient;
using Bom.Web.TestUtils;

namespace Bom.Web.Test
{
    public class SimpleTests
    {
        [Fact]
        public void CallBomApi()
        {
            using (var httpClient = BomClient.HttpClientFactory.CreateHttpClient())
            {
                var client = new BomClient.TreeClient(ConfigUtils.BaseUrl, httpClient);
                var rootNodes = client.GetRootNodes();
                Assert.NotNull(rootNodes);
            }
        }

        [Fact]
        public void GetChildPathsBySearch()
        {
            using (var httpClient = HttpClientFactory.CreateHttpClient())
            {
                var client = new TreeClient(ConfigUtils.BaseUrl, httpClient);
                var rootNode = client.GetRootNodes().Result.FirstOrDefault();
                if (rootNode == null)
                {
                    return; //  nothing to query (test is to be improved)
                }
                var input = new TreeFilterInput();
                input.BasePathId = rootNode.PathId;
                input.ChildDepth = 44;
                var response = client.GetNodes(input);
                var result = response.Result;
                foreach (var r in result)
                {
                    Console.WriteLine($"{r.NodeId}  {r.Title}");
                }
                // second serch
                var baseNode = result.FirstOrDefault(x => x.Depth > 2);
                if (baseNode != null)
                {
                    input = new TreeFilterInput();
                    input.BasePathId = baseNode.PathId;
                    input.ChildDepth = 2;
                    var response2 = client.GetNodes(input);
                }
            }
        }

        [Fact]
        public void GetParentPathsBySearch()
        {
            using (var httpClient = HttpClientFactory.CreateHttpClient())
            {
                var client = new TreeClient(ConfigUtils.BaseUrl, httpClient);
                var rootNode = client.GetRootNodes().Result.FirstOrDefault();
                if (rootNode == null)
                {
                    return; //  nothing to query (test is to be improved)
                }
                var input = new TreeFilterInput();
                input.BasePathId = rootNode.PathId;
                input.ChildDepth = 44;
                var response = client.GetNodes(input);
                // second serch
                var baseNode = response.Result.FirstOrDefault(x => x.Depth > 4);
                if (baseNode != null)
                {
                    input = new TreeFilterInput();
                    input.BasePathId = baseNode.PathId;
                    input.ParentDepth = 50;
                    var response2 = client.GetNodes(input);

                    input = new TreeFilterInput();
                    input.BasePathId = baseNode.PathId;
                    input.ParentDepth = 2;
                    var response3 = client.GetNodes(input);
                }
            }
        }


        [Fact]
        public void GetNodeInstance()
        {
            using (var httpClient = HttpClientFactory.CreateHttpClient())
            {
                var client = new TreeClient(ConfigUtils.BaseUrl, httpClient);
                var node = client.GetNodeByNodeId(1);
                var node2 = client.GetNodeByPathId(1);
            }
        }
    }
}